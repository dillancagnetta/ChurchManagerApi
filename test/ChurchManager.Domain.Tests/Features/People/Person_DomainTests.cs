using ChurchManager.Domain.Features.People;

namespace ChurchManager.Domain.Tests.Features.People;

public class Person_DomainTests
{
    [Fact]
    public void Test_base_entity_method_GetCurrentPropertyType()
    {
        var now = DateTime.UtcNow;
        var person = new Person
        {
           BaptismStatus = new Baptism
           {
               BaptismDate = now,
               IsBaptised = true
           },
            Gender = Gender.Male,
            FullName = new FullName()
            {
                FirstName = "John",
                LastName = "Doe"
            }
        };
        
        var propertyTypeBaptismDate = person.GetCurrentPropertyType("BaptismStatus.BaptismDate");
        var propertyTypeIsBaptised = person.GetCurrentPropertyType("BaptismStatus.IsBaptised");
        Assert.Equal(typeof(DateTime?), propertyTypeBaptismDate);
        Assert.Equal(typeof(Boolean?), propertyTypeIsBaptised);
        
        var propertyValueBaptismDate = person.GetCurrentPropertyValue("BaptismStatus.BaptismDate");
        var propertyValueIsBaptised = person.GetCurrentPropertyValue("BaptismStatus.IsBaptised");
        Assert.Equal(now, propertyValueBaptismDate);
        Assert.Equal(true, propertyValueIsBaptised);
    }
}