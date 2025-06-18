using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Infrastructure.Abstractions;
using Codeboss.Types;

namespace ChurchManager.Persistence.Shared
{
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        [Key]
        public TPrimaryKey Id { get; set; }
        
        [DefaultValue("Active"), MaxLength(25)]
        public string RecordStatus { get; set; } = "Active";
        
        public DateTime? InactiveDateTime { get; set; }

        #region Domain Events

        [NotMapped]
        private readonly List<IDomainEvent> _domainEvents = [];
        [NotMapped]
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        public void AddDomainEvent(IDomainEvent @event) => _domainEvents?.Add(@event);
        public void ClearDomainEvents() => _domainEvents.Clear();

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a dictionary containing the majority of the entity object's properties. The only properties that are excluded
        /// are the Id, Guid and Order.  
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> that represents the current entity object. Each <see cref="KeyValuePair{String, Object}"/> includes the property
        /// name as the key and the property value as the value.</returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            var virtualPropsWhiteList = new HashSet<string>
            {
                "CreatedBy",
                "CreatedDate",
                "ModifiedDate",
                "ModifiedBy"
            };

            foreach(var propInfo in this.GetType().GetProperties())
            {
                if((propInfo.GetGetMethod() != null && !propInfo.GetGetMethod().IsVirtual) || virtualPropsWhiteList.Contains(propInfo.Name))
                {
                    dictionary.Add(propInfo.Name, propInfo.GetValue(this, null));
                }
            }

            return dictionary;
        }
        
        public string EntityType => GetType().Name;
        
        /// <summary>
        /// Retrieves the value of a property from the entity using a dot-notation property path.
        /// This method supports navigating through nested properties (e.g., "Person.Address.City").
        /// </summary>
        /// <param name="propertyPath">
        /// A string representing the property path to navigate. 
        /// Can include dot notation for nested properties (e.g., "Person.Address.City").
        /// </param>
        /// <returns>
        /// The value of the specified property if found; otherwise, null.
        /// Returns null if any part of the path is null or if a property in the path doesn't exist.
        /// </returns>
        public object? GetCurrentPropertyValue(string propertyPath)
        {
            var parts = propertyPath.Split('.');
            object? current = this;  // start at the entity

            foreach (var part in parts)
            {
                if (current == null) return null;
            
                var property = current.GetType().GetProperty(part);
                if (property == null) return null;
            
                current = property.GetValue(current);
            }

            return current;
        }
        
        /// <summary>
        /// Retrieves the Type of a property from the entity using a dot-notation property path.
        /// This method supports navigating through nested properties (e.g., "Person.Address.City").
        /// </summary>
        /// <param name="propertyPath">
        /// A string representing the property path to navigate. 
        /// Can include dot notation for nested properties (e.g., "Person.Address.City").
        /// </param>
        /// <returns>
        /// The Type of the specified property if found; otherwise, null.
        /// Returns null if any part of the path is null or if a property in the path doesn't exist.
        /// </returns>
        public Type? GetCurrentPropertyType(string propertyPath)
        {
            var parts = propertyPath.Split('.');
            object? current = this; // start at the entity
            Type? type = null;

            foreach (var part in parts)
            {
                if (current == null) return null;

                type = current.GetType();
                var property = type.GetProperty(part);
                if (property == null) return null;
                
                // If this is the last part in the path, return the property's type
                if (part == parts[^1])
                {
                    return property.PropertyType;
                }
                
                current = property.GetValue(current);
            }

            return null; // Should not reach here if the path is valid
        }

        #endregion
    }
}