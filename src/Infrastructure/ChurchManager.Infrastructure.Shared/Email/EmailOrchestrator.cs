using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Infrastructure.Abstractions.Communication;
using ChurchManager.Infrastructure.Shared.Templating;
using Codeboss.Results;
using DotLiquid;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Infrastructure.Shared.Email;

public class EmailOrchestrator : IEmailOrchestrator
{
    private readonly IEmailSender _sender;
    private readonly ITemplateParser _templateParser;
    private readonly ITemplateDbRepository _templateDb;
    private readonly ITemplateDataResolverFactory _templateDataFactory;
    private readonly IDistributedCache _cache;
    public ILogger<EmailOrchestrator> Logger { get; }
    
    public EmailOrchestrator(
        IEmailSender sender,
        ITemplateParser templateParser,
        ITemplateDbRepository templateDb,
        ITemplateDataResolverFactory templateDataFactory,
        IDistributedCache cache,
        ILogger<EmailOrchestrator> logger)
    {
        _sender = sender;
        _templateParser = templateParser;
        _templateDb = templateDb;
        _templateDataFactory = templateDataFactory;
        _cache = cache;
        Logger = logger;
    }

    public async Task<OperationResult> SendEmailAsync(EmailRecipient recipient, string subject , TemplateInfo templateInfo, CancellationToken ct = default)
    {
        try
        {
            var template = await _templateDb.TemplateByNameAsync(templateInfo.TemplateName, ct);
            Template.FileSystem = new DatabaseTemplateFileSystem(_templateDb, _cache);
                
            var resolver = _templateDataFactory.CreateResolver(templateInfo.TemplateName);
            var templateData = await resolver.ResolveDataAsync(recipient.PersonId, templateInfo.TemplateData, ct);
            object model = new { Model = templateData };

            var htmlBody = _templateParser.Render(template.Content, model);

            var operationResult = await _sender.SendEmailAsync(recipient, subject, htmlBody);
            
            return operationResult;

        } catch (Exception ex)
        {
            Logger.LogError($"Error sending email: {ex.Message}");
            return OperationResult.Fail($"Error sending email: {ex.Message}");
        }
    }
}