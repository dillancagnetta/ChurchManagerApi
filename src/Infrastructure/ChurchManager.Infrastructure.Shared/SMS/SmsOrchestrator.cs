using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Communication;
using ChurchManager.Infrastructure.Shared.Templating;
using Codeboss.Results;
using DotLiquid;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Infrastructure.Shared.SMS;

public class SmsOrchestrator(
            ISmsSender sender,
            ITemplateParser templateParser,
            ITemplateDbRepository templateDb,
            ITemplateDataResolverFactory templateDataFactory,
            IDistributedCache cache,
            ILogger<SmsOrchestrator> logger): ISmsOrchestrator
{
    
    public async Task<OperationResult<IEnumerable<SmsOperationResult>>> SendBulkSmsAsync(BulkSmsMessage message, CancellationToken ct = default)
    {
        try
        {
            var operationResult = await sender.SendSmsAsync(message, ct);
            
            return operationResult;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    

    public async Task<OperationResult<SmsOperationResult>> SendSmsAsync(SmsMessage message, TemplateInfo templateInfo, CancellationToken ct = default)
    {
        try
        {
            var template = await templateDb.TemplateByNameAsync(templateInfo.TemplateName, ct);
            Template.FileSystem = new DatabaseTemplateFileSystem(templateDb, cache);
            
            var resolver = templateDataFactory.CreateResolver(templateInfo.TemplateName);
            var templateData = await resolver.ResolveDataAsync(
                message.Recipient.PersonId, additionalData:templateInfo.TemplateData, ct);
            object model = new { Model = templateData };

            var smsBody = templateParser.Render(template.Content, model);
            message.Body = smsBody;
            
            var operationResult = await sender.SendSmsAsync(message, ct);
            
            return operationResult;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async IAsyncEnumerable<OperationResult<SmsOperationResult>> SendSmsAsync(IEnumerable<SmsMessage> messages, TemplateInfo templateInfo, CancellationToken ct = default)
    {
        foreach (var message in messages)
        {
            if (ct.IsCancellationRequested) yield break;
            
            OperationResult<SmsOperationResult> result;
            try
            {
                result = await SendSmsAsync(message, templateInfo, ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending SMS to {PersonId}", message.Recipient.PersonId);
                result = new OperationResult<SmsOperationResult>(new SmsOperationResult
                {
                    PersonId = message.Recipient.PersonId,
                    IsSent = false,
                    Error = ex.Message
                });
            }

            yield return result;
        }
    }
}