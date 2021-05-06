﻿using System;
using System.Collections.Generic;
using System.Linq;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Model
{
    public class PersonDomain
    {
        private readonly Person _entity;
        public int PersonId => _entity.Id;
        public int? FamilyId => _entity.FamilyId;
        public ChurchDomain Church => new (_entity.Church);
        public ConnectionStatus ConnectionStatus => _entity.ConnectionStatus ?? ConnectionStatus.Unknown;
        public RecordStatus RecordStatus => _entity.RecordStatus;
        public AgeClassification AgeClassification => _entity.AgeClassification;
        public FullName FullName => _entity.FullName;
        public BirthDate BirthDate => _entity.BirthDate;
        public Gender Gender => _entity.Gender;
        public Baptism BaptismStatus => _entity.BaptismStatus;
        public DateTime? FirstVisitDate => _entity.FirstVisitDate;
        public string MaritalStatus => _entity.MaritalStatus;
        public DateTime? AnniversaryDate => _entity.AnniversaryDate;
        public Email Email => _entity.Email;
        public ICollection<PhoneNumber> PhoneNumbers => _entity.PhoneNumbers;
        public string CommunicationPreference => _entity.CommunicationPreference;
        public string PhotoUrl => _entity.PhotoUrl;
        public string Occupation => _entity.Occupation;
        public string Source => _entity.Source;
        public bool? ReceivedHolySpirit => _entity.ReceivedHolySpirit;
        public DiscipleshipStepViewModel FoundationSchool { get; set; }
        public string UserLoginId => _entity.UserLoginId;

        // Gets the persons family members excluding them
        public ICollection<FamilyMemberDomain> FamilyMembers
            => _entity.Family?.FamilyMembers?
                .Where(x => x.Id != PersonId)
                .Select(x => new FamilyMemberDomain(x))
                .ToList();

        public PersonDomain(Person entity)
        {
            _entity = entity;

            /*var newConvertsDiscipleshipProgram =
                _entity.DiscipleshipPrograms.FirstOrDefault(x => x.Name == "New Converts Program");
            var foundationSchoolStep = newConvertsDiscipleshipProgram?.DiscipleshipSteps.FirstOrDefault();

            FoundationSchool = new()
            {
                IsCompleted = foundationSchoolStep != null && foundationSchoolStep.Status == "Completed",
                Status = foundationSchoolStep?.Status,
                CompletionDate = foundationSchoolStep?.CompletionDate
            };*/
        }
    }
    
    public class FamilyMemberDomain
    {
        private readonly Person _entity;
        public int PersonId => _entity.Id;
        public FullName FullName => _entity.FullName;
        public AgeClassification AgeClassification => _entity.AgeClassification;
        public Gender Gender => _entity.Gender;
        public BirthDate BirthDate => _entity.BirthDate;
        public string PhotoUrl => _entity.PhotoUrl;

        public FamilyMemberDomain(Person entity) => _entity = entity;
    }
}
