using System.Reflection;
using System.Text.Json;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.ChangeRequests;
using ChurchManager.Domain.Features.ChangeRequests.Events;
using ChurchManager.Domain.Features.People;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wolverine;

namespace ChurchManager.Features.Common.Events.ChangeRequestApproved;

public class ChangeRequestApprovedConsumer(
    IGenericDbRepository<ChangeRequest> dbRepository,
    IGenericDbRepository<Person> personDb,
    ILogger<ChangeRequestApprovedConsumer> logger) : IDomainEventHandler
{
    public async Task Handle(ChangeRequestApprovedEvent message, IMessageContext context, CancellationToken ct)
    {
        logger.LogInformation("✔️------ ChangeRequestApprovedEvent event received ------");
        var changeRequestId = message.ChangeRequestId;
        var changeRequest = await dbRepository
            .Queryable()
            .Include(x => x.Properties)
            .FirstOrDefaultAsync(x =>
                    x.Id == changeRequestId &&
                    x.Properties.Any(p => p.IsApplied == false)
                , ct);
        
        if (changeRequest is not null)
        {
            try
            {
                // Group property changes by entity type and ID to minimize database calls
                var entityGroups = changeRequest.Properties
                    .Where(p => p.IsApplied == false)
                    .GroupBy(p => new { p.EntityType, p.EntityId })
                    .ToList();

                foreach (var entityGroup in entityGroups)
                {
                    await ApplyPropertyChangesToEntity(entityGroup.Key.EntityType, entityGroup.Key.EntityId, entityGroup.ToList());
                }
                
                // Mark all property changes as applied
                await UpdatePropertyAppliedStatus(changeRequest, ct);
            
                logger.LogInformation("Successfully applied {PropertyCount} property changes across {EntityCount} entities for ChangeRequest {Id}", 
                    changeRequest.Properties.Count, entityGroups.Count, changeRequestId);
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to apply change request {Id}", changeRequestId);
                throw;
            }
        }
        
    }

    private async Task UpdatePropertyAppliedStatus(ChangeRequest changeRequest, CancellationToken ct)
    {
        foreach (var property in changeRequest.Properties.Where(p => p.IsApplied == false))
        {
            property.IsApplied = true;
        }
        await dbRepository.UpdateAsync(changeRequest, ct);
        await dbRepository.SaveChangesAsync(ct);
    }

    private async Task ApplyPropertyChangesToEntity(string entityType, int entityId, List<PropertyChangeRequest> propertyChanges)
    {
        if (entityType == nameof(Person))
        {
            // Get from the database
            var person = await personDb.GetByIdAsync(entityId);
            if (person != null)
            {
                // Apply all property changes to this person in one go
                foreach (var propertyChange in propertyChanges)
                {
                    await ApplyPropertyChange(person, propertyChange);
                }
                
                // Save the person once after all changes are applied
                await personDb.UpdateAsync(person);
                
                logger.LogDebug("Applied {Count} property changes to Person {PersonId}", 
                    propertyChanges.Count, entityId);
            }
            else
            {
                logger.LogWarning("Person {PersonId} not found for property changes", entityId);
            }
        }
        // Add other entity types as needed
        // else if (entityType == nameof(Family)) 
        // {
        //     var family = await familyDb.GetByIdAsync(entityId);
        //     if (family != null)
        //     {
        //         foreach (var propertyChange in propertyChanges)
        //         {
        //             await ApplyPropertyChange(family, propertyChange);
        //         }
        //         await familyDb.UpdateAsync(family);
        //     }
        // }
        else
        {
            logger.LogWarning("Unsupported entity type: {EntityType}", entityType);
        }
    }
    
    private async Task ApplyPropertyChange(object entity, PropertyChangeRequest propertyChange)
    {
        var parts = propertyChange.PropertyPath.Split('.');
        object current = entity;

        // Navigate to the parent object
        for (int i = 0; i < parts.Length - 1; i++)
        {
            var property = current!.GetType().GetProperty(parts[i]);
            if (property == null)
                throw new InvalidOperationException($"Property {parts[i]} not found");

            var value = property.GetValue(current);
            if (value == null)
            {
                // Create instance for owned types
                if (IsOwnedType(property.PropertyType))
                {
                    value = Activator.CreateInstance(property.PropertyType);
                    property.SetValue(current, value);
                }
                else
                {
                    throw new InvalidOperationException($"Cannot set property on null object: {parts[i]}");
                }
            }
            current = value;
        }

        // Set the final property value
        var finalProperty = current.GetType().GetProperty(parts.Last());
        if (finalProperty == null)
            throw new InvalidOperationException($"Property {parts.Last()} not found");

        var requestedValue = DeserializePropertyValue(propertyChange.RequestedValue, finalProperty.PropertyType);
        finalProperty.SetValue(current, requestedValue);
    }
    
    private object? DeserializePropertyValue(string? jsonValue, Type targetType)
    {
        if (string.IsNullOrEmpty(jsonValue))
            return null;

        try
        {
            return JsonSerializer.Deserialize(jsonValue, targetType);
        }
        catch
        {
            // Fallback for simple types
            return Convert.ChangeType(JsonSerializer.Deserialize<object>(jsonValue), targetType);
        }
    }
    
    private bool IsOwnedType(Type type)
    {
        return type.GetCustomAttribute<OwnedAttribute>() != null ||
               type == typeof(FullName) ||
               type == typeof(BirthDate) ||
               type == typeof(Baptism) ||
               type == typeof(Email) ||
               type == typeof(DeceasedStatus);
    }

}