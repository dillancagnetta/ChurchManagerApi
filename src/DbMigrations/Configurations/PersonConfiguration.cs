﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using People.Domain;
using People.Persistence.Models;
using Shared.Kernel;
using MaritalStatus = People.Domain.MaritalStatus;

namespace DbMigrations.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder
                .Property(e => e.RecordStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => new RecordStatus(v) );

            builder
                .Property(e => e.ConnectionStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => new ConnectionStatus(v) );

            builder
                .Property(e => e.AgeClassification)
                .HasConversion(
                    v => v.ToString(),
                    v => new AgeClassification(v));

            builder
                .Property(e => e.Gender)
                .HasConversion(
                    v => v.ToString(),
                    v => new Gender(v));

            builder
                .Property(e => e.MaritalStatus.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => new MaritalStatus(v));

            builder
                .Property(e => e.CommunicationPreference)
                .HasConversion(
                    v => v.ToString(),
                    v => new CommunicationType(v));
        }
    }
}