using System.ComponentModel.DataAnnotations;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.People.Services;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ChurchManager.Features.Events.Commands;

public record AddEventCommand : IRequest<ApiResponse>
{
    [Required]
    public string Name { get; set; }
        
    public string Description { get; set; }
        
    public int EventTypeId { get; set; }
        
    public int? ChildCareGroupId { get; set; }
    
    public int? EventRegistrationGroupId { get; set; }
        
    public int ContactPersonId { get; set; }
        
    public string ContactEmail { get; set; }
        
    public string ContactPhone { get; set; }
    public string Location { get; set; }
    public int? Capacity { get; set; }
        
    public int? ChurchGroupId { get; set; }
    public int? ChurchId { get; set; }

        
    public List<EventSessionViewModel> Sessions { get; set; } = new();
    
    public IFormFile Image { get; set; }
}

public class AddEventCommandHandler(
    IEventService service,
    IReadDbRepository<GroupType> groupTypesDb,
    IPhotoService photos,
    IWebHostEnvironment host
    ) : IRequestHandler<AddEventCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(AddEventCommand command, CancellationToken ct)
    {
        var entity = new Event
        {
            Name = command.Name,
            Description = command.Description,
            EventTypeId = command.EventTypeId,
            ChildCareGroupId = command.ChildCareGroupId,
            EventRegistrationGroupId = command.EventRegistrationGroupId,
            ContactPersonId = command.ContactPersonId,
            ContactEmail = command.ContactEmail,
            ContactPhone = command.ContactPhone,
            Capacity = command.Capacity,
            ChurchGroupId = command.ChurchGroupId,
            ChurchId = command.ChurchId,
            Location = command.Location,
            ApprovalStatus = ApprovalStatus.PendingApproval.Value,
            Sessions = command.Sessions.Select(x => new EventSession
            {
                Name = x.Name,
                Description = x.Description,
                SessionOrder = x.SessionOrder,
                AttendanceRequired = x.AttendanceRequired,
                OnlineSupport = x.OnlineSupport,
                OnlineMeetingUrl = x.OnlineMeetingUrl,
                Capacity = x.Capacity,
                Location = x.Location,
                Schedule = new Schedule
                {
                    Name = $"{x.Name}-EventSession",
                    StartDate = x.StartDate,
                    StartTime = TimeSpan.Parse(x.StartTime),
                    EndDate = x.EndDate,
                    EndTime =  TimeSpan.Parse(x.EndTime),
                    Timezone = "South Africa Standard Time"
                }
            }).ToList()
        };
        
        if (command.Image is { Length: > 0 })
        {
            var environment = host.EnvironmentName;
            var fileName = $"{entity.EventTypeId}-{entity.Name}--{environment}";
            var operationResult = await photos.AddImageAsync(fileName, command.Image, "events", ct:ct);
            if (operationResult.IsSuccess)
            {
                entity.PhotoUrl = operationResult.Result;
            } else throw new Exception(operationResult.Errors.First().Message);
        }

        var vm = await service.AddAsync(entity, ct);
        
        return new ApiResponse(vm);
    }
}