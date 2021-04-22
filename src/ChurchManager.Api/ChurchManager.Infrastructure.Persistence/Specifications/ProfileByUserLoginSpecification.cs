﻿using ChurchManager.Persistence.Models.People;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Infrastructure.Persistence.Specifications
{
    public class ProfileByUserLoginSpecification : Specification<Person>
    {
        public ProfileByUserLoginSpecification(string userLoginId)
        {
            Criteria = x => x.UserLoginId == userLoginId;

            Includes.Add(x => x.Church);
            Includes.Add(x => x.PhoneNumbers);
            IncludeStrings.Add("Family.FamilyMembers");
            IncludeStrings.Add("DiscipleshipPrograms.DiscipleshipSteps");
        }
    }

    public class ProfileByPersonSpecification : Specification<Person>
    {
        public ProfileByPersonSpecification(int personId)
        {
            Criteria = x => x.Id == personId;

            Includes.Add(x => x.Church);
            Includes.Add(x => x.PhoneNumbers);
            IncludeStrings.Add("Family.FamilyMembers");
            IncludeStrings.Add("DiscipleshipPrograms.DiscipleshipSteps");
        }
    }
}
