using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Finances;

// Benefactor is now a wrapper around existing entities
public class Benefactor: AuditableEntity<int>, IAggregateRoot<int>
{
    [Required] public BenefactorType Type { get; set; }
    [Required, MaxLength(255)] public required string Name { get; set; }
    public string? TaxId { get; private set; }
        
    // Reference IDs to original entities
    public int? PersonId { get; private set; }
    public int? FamilyId { get; private set; }
    public int? GroupId { get; private set; }
    public int? ChurchId { get; private set; }

    #region Navigation

    public virtual Person? Person { get; set; }
    public virtual Family? Family { get; set; }
    public virtual Group? Group { get; set; }
    public virtual Church? Church { get; set; }

    #endregion
    
    // Factory methods for different donor types
    public static Benefactor FromPerson(Person person)
    {
        return new Benefactor
        {
            Type = BenefactorType.Individual,
            Name = person.FullName!.ToString(),
            PersonId = person.Id
        };
    }
    
    public static Benefactor FromFamily(Family family)
    {
        return new Benefactor
        {
            Type = BenefactorType.Family,
            Name = family.Name,
            FamilyId = family.Id,
            // Other mappings
        };
    }
        
    public static Benefactor FromGroup(Group group)
    {
        return new Benefactor
        {
            Type = BenefactorType.Group,
            Name = group.Name,
            GroupId = group.Id,
            // Other mappings
        };
    }
    public static Benefactor FromChurch(Church church)
    {
        return new Benefactor
        {
            Type = BenefactorType.Church,
            Name = church.Name,
            GroupId = church.Id,
            // Other mappings
        };
    }
}