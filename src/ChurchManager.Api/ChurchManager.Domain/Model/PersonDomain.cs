﻿using System;
using System.Collections.Generic;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Models.People;

namespace ChurchManager.Domain.Model
{
    public class PersonDomain
    {
        private readonly Person _entity;
        public int PersonId => _entity.Id;
        public int? ChurchId => _entity.ChurchId;
        public int? FamilyId => _entity.FamilyId;
        public ConnectionStatus ConnectionStatus => _entity.ConnectionStatus;
        public RecordStatus RecordStatus => _entity.RecordStatus;
        public AgeClassification AgeClassification => _entity.AgeClassification;
        public FullName FullName => _entity.FullName;
        public BirthDate BirthDate => _entity.BirthDate;
        public Gender Gender => _entity.Gender;
        public DateTime? FirstVisitDate => _entity.FirstVisitDate;
        public string MaritalStatus => _entity.MaritalStatus;
        public DateTime? AnniversaryDate => _entity.AnniversaryDate;
        public Email Email => _entity.Email;
        public ICollection<PhoneNumber> PhoneNumbers => _entity.PhoneNumbers;
        public string CommunicationPreference => _entity.CommunicationPreference;
        public string PhotoUrl => _entity.PhotoUrl;
        public string Occupation => _entity.Occupation;
        public bool? ReceivedHolySpirit => _entity.ReceivedHolySpirit;

        public PersonDomain(Person entity) => _entity = entity;
    }
}