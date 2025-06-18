using ChurchManager.Domain.Features.Communications.Events;
using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Domain.Features.People;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Abstractions.Communication;
using ChurchManager.Infrastructure.Shared.Templating;
using CodeBoss.Extensions;
using DotLiquid;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

// Add this if you're using DotLiquid

// https://github.com/dotliquid/dotliquid/blob/master/src/DotLiquid/Tags/Extends.cs

namespace ChurchManager.Features.Communication.Events.SendEmail
{
    public class SendEmailConsumer : IDomainEventHandler
    {
        private readonly IEmailSender _sender;
        private readonly ITemplateParser _templateParser;
        private readonly ITemplateDbRepository _templateDb;
        private readonly IDistributedCache _cache;
        public ILogger<SendEmailConsumer> Logger { get; }

        public SendEmailConsumer(
            IEmailSender sender,
            ITemplateParser templateParser,
            ITemplateDbRepository templateDb,
            IDistributedCache cache,
            ILogger<SendEmailConsumer> logger)
        {
            _sender = sender;
            _templateParser = templateParser;
            _templateDb = templateDb;
            _cache = cache;
            Logger = logger;
        }

        public async Task Handle(SendEmailEvent message, CancellationToken ct)
        {
            Logger.LogInformation("✔️------ SendEmailConsumer event received ------");
            
            try
            {
                if(!message.Recipient.EmailAddress.IsNullOrEmpty())
                {
                    //var path = DomainConstants.Communication.Email.Template(message.Template);
                    //string template = await File.ReadAllTextAsync(path);
                    // Set up the file system for includes
                    //Template.FileSystem = new LocalFileSystem(DomainConstants.Communication.Email.TemplatePath);
                
                    var template = await _templateDb.TemplateByNameAsync(message.Template, ct);
                    Template.FileSystem = new DatabaseTemplateFileSystem(_templateDb, _cache);
                
                    object model = new { Model = message.TemplateData };

                    var htmlBody = _templateParser.Render(template.Content, model);

                    var operationResult = await _sender.SendEmailAsync(message.Recipient, message.Subject, htmlBody);

                    if (operationResult.IsSuccess)
                    {
                    
                    }
                    else
                    {
                        // Raise event to set their email address to inactive to avoid repeated bounces
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error sending email: {ex.Message}");
                throw;
            }
        }

        private readonly Predicate<Person> _isEmailActive = person => person.Email?.IsActive != null && person.Email.IsActive.Value;
    }
}