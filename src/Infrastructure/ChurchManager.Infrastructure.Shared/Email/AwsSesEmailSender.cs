using System.Net;
using Amazon;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Services;
using CodeBoss.Extensions;
using Codeboss.Results;
using Message = Amazon.SimpleEmailV2.Model.Message;

namespace ChurchManager.Infrastructure.Shared.Email
{
    public class AwsSesEmailSender : IEmailSender
    {
        private readonly string _awsAccessKeyId;
        private readonly string _awsSecretAccessKey;

        public AwsSesEmailSender(string awsAccessKeyId, string awsSecretAccessKey)
        {
            _awsAccessKeyId = awsAccessKeyId;
            _awsSecretAccessKey = awsSecretAccessKey;
        }

        public async Task<OperationResult> SendEmailAsync(EmailRecipient recipient, string subject, string htmlBody)
        {
            if(recipient.EmailAddress.IsNullOrEmpty()) return OperationResult.Fail("Email address is required");
                
            try
            {
                // Change to your from email
                string senderAddress = "connect@codeboss.co.za";
                // Change to your region
                using var client = new AmazonSimpleEmailServiceV2Client(_awsAccessKeyId, _awsSecretAccessKey, RegionEndpoint.USEast1);
                var sendRequest = new SendEmailRequest
                {
                    FromEmailAddress = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { recipient.EmailAddress }
                    },
                    Content = new EmailContent
                    {
                        Simple = new Message
                        {
                            Subject = new Content {Data = subject},
                            Body = new Body
                            {
                                Html = new Content
                                {
                                    Charset = "UTF-8",
                                    Data = htmlBody
                                },
                                Text = new Content
                                {
                                    Charset = "UTF-8",
                                    Data = htmlBody
                                }
                            }
                        }
                    }
                };

                var response = await client.SendEmailAsync(sendRequest);
                
                switch (response.HttpStatusCode)
                {
                    case HttpStatusCode.OK:
                        return OperationResult.FromResult(response.MessageId);
                    case HttpStatusCode.BadRequest:
                        return OperationResult.Fail("Bad request. Please check your email parameters.");
                    case HttpStatusCode.Forbidden:
                        return OperationResult.Fail("You don't have permission to send emails.");
                    case HttpStatusCode.TooManyRequests:
                        return OperationResult.Fail("Sending limit exceeded. Please try again later.");
                    default:
                        return OperationResult.Fail($"Failed to send email. Status code: {response.HttpStatusCode}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult.Fail($"Failed to send email: {ex.Message}");
            }
        }

        /*public Task<OperationResult> SendEmailAsync(Domain.Features.People.Email email, string subject, string htmlBody)
        {
            if (email.IsActive.HasValue && email.IsActive.Value)
            {
                return SendEmailAsync(email.Address, subject, htmlBody);
            }

            return Task.FromResult(OperationResult.Success());
        }

        public Task<OperationResult> SendEmailAsync(Person person, string subject, string htmlBody)
        {
            if(person.Email.IsActive.HasValue && person.Email.IsActive.Value)
            {
                return SendEmailAsync(person.Email.Address, subject, htmlBody);
            }

            return Task.FromResult(OperationResult.Success());
        }*/
    }
}
