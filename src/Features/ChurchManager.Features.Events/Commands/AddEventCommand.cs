using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.People.Services;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CodeBoss.Extensions;

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

/*
 * ------------------------------------------------
 */
 
 public record EditEventCommand : AddEventCommand, IRequest<ApiResponse>
{
    public int Id { get; set; }
}

public class EditEventCommandCommandHandler(
    IEventService service,
    IGenericDbRepository<Event> dbRepository,
    IPhotoService photos,
    IMapper mapper,
    IWebHostEnvironment host
    ) : IRequestHandler<EditEventCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(EditEventCommand command, CancellationToken ct)
    {
        var eventEntity  = await dbRepository.Queryable()
            .Include(x => x.Sessions)
                .ThenInclude(x => x.Schedule)
            .SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        
        if (eventEntity == null) return new ApiResponse("Event not found");
        
        // Step 1: Remove Sessions that are missing in the DTO
        var removedSessions = eventEntity.Sessions
            .Where(s => !command.Sessions.Any(dto => dto.Id == s.Id))
            .ToList();
        
        foreach (var session in removedSessions)
        {
            eventEntity.Sessions.Remove(session);
        }
        
        // Photo Removed
        if (eventEntity.HasPhoto && command.Image is null)
        {
            if (eventEntity.PhotoUrl.Contains("cloudinary", StringComparison.InvariantCultureIgnoreCase))
            {
                var publicId = eventEntity.PhotoUrl.CloudinaryPublicId();
                await photos.DeletePhotoAsync(publicId);
            }
            eventEntity.PhotoUrl = null;
        }
        // Update Photo
        if (command.Image is { Length: > 0 })
        {
            // Delete current photo
            if(eventEntity.HasPhoto && eventEntity.PhotoUrl.Contains("cloudinary", StringComparison.InvariantCultureIgnoreCase))
            {
                var publicId = eventEntity.PhotoUrl.CloudinaryPublicId();
                await photos.DeletePhotoAsync(publicId);
            }
            
            // Add new photo to Cloudinary
            var fileName = GenerateFileName(eventEntity);
            var operationResult = await photos.AddImageAsync(fileName, command.Image, "events", ct:ct);
            if (operationResult.IsSuccess)
            {
                eventEntity.PhotoUrl = operationResult.Result;
            } else throw new Exception(operationResult.Errors.First().Message);
        }

        // Step 2: Update existing sessions without replacing the collection
        foreach (var sessionDto in command.Sessions.Where(s => s.Id is not (null or 0)))
        {
            var existingSession = eventEntity.Sessions.FirstOrDefault(s => s.Id == sessionDto.Id);
            if (existingSession != null)
            {
                mapper.Map(sessionDto, existingSession);  // Map properties only
            }
        }
        
        // Step 3: Add new Sessions (Id == 0 or null)
        var newSessions = command.Sessions
            .Where(s => s.Id is null or 0)
            .Select(sessionDto =>
            {
                var newSession = mapper.Map<EventSession>(sessionDto); // Map basic properties
                // Create a Schedule entity from sessionDto
                newSession.Schedule = new Schedule
                {
                    Name = $"{eventEntity.Name}-EventSession",
                    StartDate = sessionDto.StartDate,
                    EndDate = sessionDto.EndDate,
                    StartTime = TimeSpan.Parse(sessionDto.StartTime),
                    EndTime = TimeSpan.Parse(sessionDto.EndTime),
                    Timezone = "South Africa Standard Time"
                };
                return newSession;
            }).ToList();
        
        foreach (var newSession in newSessions)
        {
            eventEntity.Sessions.Add(newSession);
        }

        // Step 4: Map other properties (Sessions already handled)
        mapper.Map(command, eventEntity); 
        
        await dbRepository.SaveChangesAsync(ct); 
        
        return new ApiResponse();
    }

    private string GenerateFileName(Event entity)
    {
        var environment = host.EnvironmentName;
        var fileName = $"{entity.EventTypeId}-{entity.Name}--{environment}";
        return fileName;
    }
}