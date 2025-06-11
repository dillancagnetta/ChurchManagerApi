using System.Reflection;
using System.Text.Json;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.ChangeRequests;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.Common.Services;

public class ChangeRequestService(
    IPersonDbRepository personDb,
    IGenericDbRepository<ChangeRequest> dbRepository,
    ILogger<ChangeRequestService> logger) : IChangeRequestService
{
    public async Task<OperationResult<ChangeRequest>> CreatePersonChangeRequestAsync(int personId, Dictionary<string, object?> propertyChanges,
        string? reason = null)
    {
        // Person exists check
        var person = await personDb.GetByIdAsync(personId);
        if (person == null) return OperationResult<ChangeRequest>.Fail("Person not found");

        // Authorization check
        /*
        if (!await CanUserUpdatePerson(requestedByPersonId, personId))
            return OperationResult<ChangeRequest>.Fail("Unauthorized to modify this person");
            */

        var changeRequest = new ChangeRequest
        {
            Reason = reason,
            Source = "FamilyPortal",
            ChurchId = person.ChurchId
        };
        var changeRequests = new List<PropertyChangeRequest>(propertyChanges.Count);

        foreach (var (propertyPath, requestedValue) in propertyChanges)
        {
            var currentValue = person.GetCurrentPropertyValue(propertyPath);
            var currentType = person.GetCurrentPropertyType(propertyPath);
            
            var propertyChange = new PropertyChangeRequest
            {
                
                EntityType = nameof(Person),
                EntityId = personId,
                PropertyPath = propertyPath,
            };
            
            propertyChange.SetCurrentValue(currentValue, currentType);
            propertyChange.SetRequestedValue(requestedValue);
            
            // Only add if there's an actual change
            if (propertyChange.HasActualChange())
            {
                changeRequests.Add(propertyChange);
            }
        }

        if (!changeRequests.Any()) return OperationResult<ChangeRequest>.Fail("No changes detected");

        // Attach property changes to the change request and save
        changeRequest.Properties = changeRequests;
        await dbRepository.AddAsync(changeRequest);
        await dbRepository.SaveChangesAsync();

        logger.LogInformation("Created {Count} change requests for Person {PersonId}", 
            changeRequests.Count, personId);

        return OperationResult<ChangeRequest>.Success(changeRequest);
    }

    public async Task<OperationResult<ChangeRequest>> CreateBaptismChangeRequestAsync(int personId, bool? isBaptised,
        DateTime? baptismDate = null, string? reason = null)
    {
        // "BaptismStatus.IsBaptised"
        var changes = new Dictionary<string, object?>
        {
            [DomainConstants.ChangeRequest.Baptism.IsBaptised] = isBaptised
        };

        if (baptismDate.HasValue)
        {
            // "BaptismStatus.BaptismDate"
            changes[DomainConstants.ChangeRequest.Baptism.BaptismDate] = baptismDate;
        }
        
        return await CreatePersonChangeRequestAsync(personId, changes, reason);
    }

    public async Task<OperationResult<ChangeRequest>> CreateHolySpiritChangeRequestAsync(int personId, bool receivedHolySpirit,
        string? reason = null)
    {
        var changes = new Dictionary<string, object?>
        {
            [nameof(Person.ReceivedHolySpirit)] = receivedHolySpirit
        };

        return await CreatePersonChangeRequestAsync(personId, changes, reason);
    }

    #region Private Methods

    private object? GetCurrentPropertyValue(object entity, string propertyPath)
    {
        var parts = propertyPath.Split('.');
        object? current = entity;

        foreach (var part in parts)
        {
            if (current == null) return null;
            
            var property = current.GetType().GetProperty(part);
            if (property == null) return null;
            
            current = property.GetValue(current);
        }

        return current;
    }

    private async Task<bool> CanUserUpdatePerson(int requestedByPersonId, int targetPersonId)
    {
        // Implement your authorization logic here
        // Examples:
        // - Same person can update themselves
        // - Family members can update family members
        // - Church admins can update anyone
        
        /*
        if (requestedByPersonId == targetPersonId)
            return true;
            
        // Check if they're in the same family
        var requestor = await _personRepository.GetByIdAsync(requestedByPersonId);
        var target = await _personRepository.GetByIdAsync(targetPersonId);
        
        if (requestor?.FamilyId != null && requestor.FamilyId == target?.FamilyId)
            return true;*/
            
        return true; // Default to deny
    }

    #endregion
}