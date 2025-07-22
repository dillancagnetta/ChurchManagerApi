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
    [MaxLength(10)] public string? PhoneNumber { get; set; }
    public string? TaxId { get; private set; }
        
    // Reference IDs to original entities
    public int? PersonId { get;  set; }
    public int? FamilyId { get;  set; }
    public int? GroupId { get; set; }
    public int? ChurchId { get;  set; }

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
            ChurchId = church.Id,
            // Other mappings
        };
    }
    
    public static Benefactor CreateTest()
    {
        return new Benefactor
        {
            Type = BenefactorType.Individual,
            Name =  "Test Benefactor",
            PersonId = 1,
            PhoneNumber = "0712345678",
            // Other mappings
        };
    }

    public static Benefactor FromGivingReference(GivingReference reference)
    {
        switch (reference.BenefactorType.Value)
        {
            case "Individual":
                return new Benefactor{Name = reference.Person!.Name, PersonId = reference.Person!.Id, Type = BenefactorType.Individual};
            case "Family":
                return new Benefactor{Name = reference.Family!.Name, PersonId = reference.Family!.Id, Type = BenefactorType.Family};
            case "Church":
                return new Benefactor{Name = reference.Church!.Name, PersonId = reference.Church!.Id, Type = BenefactorType.Church};
     
            default:
                throw new ArgumentOutOfRangeException(nameof(reference.BenefactorType), reference.BenefactorType, "Invalid benefactor type");
        }
    }
}