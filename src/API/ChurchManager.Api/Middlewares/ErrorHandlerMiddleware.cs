﻿using System.Net;
using System.Text.Json;
using Bugsnag;
using Bugsnag.Payload;
using ChurchManager.Infrastructure.Shared.Bugsnag;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Exceptions;
using ChurchManager.SharedKernel.Wrappers;
using Microsoft.Extensions.Options;
using Exception = System.Exception;

namespace ChurchManager.Api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IOptions<BugsnagOptions> options)
        {
            try
            {
                await _next(context);
            }
            catch(Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new ApiResponse() { Succeeded = false, Message = error?.Message };

                switch(error)
                {
                    // Skip: see https://github.com/dotnet/aspnetcore/issues/23949
                    case BadHttpRequestException e:
                        _logger.LogWarning(e, "BadHttpRequestException skipped");
                        return;
                    case ApiException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case ValidationException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                _logger.LogError(error, "Application Error");

                if (options.Value.Enabled)
                {
                    var bugsnagClient = context.RequestServices.GetRequiredService<IClient>();
                    var currentUser = context.RequestServices.GetService<ICognitoCurrentUser>();

                    if (currentUser is not null)
                    {
                        var person = await currentUser.CurrentPerson.Value;
                        bugsnagClient.BeforeNotify(report =>
                        {
                            report.Event.User = new User
                            {
                                Id = currentUser.Id,
                                Name = $"{person.FullName.FirstName} {person.FullName.LastName}",
                                Email = person.Email?.Address
                            };
                        });
                    }

                    bugsnagClient.Notify(error);
                }

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
