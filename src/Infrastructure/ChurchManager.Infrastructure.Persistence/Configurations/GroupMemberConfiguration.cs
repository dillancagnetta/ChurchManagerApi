﻿#region

using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Configurations
{
    public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
    {
        public void Configure(EntityTypeBuilder<GroupMember> builder)
        {
            builder
                .Property(e => e.RecordStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => new RecordStatus(v) );

            builder
                .Property(e => e.CommunicationPreference)
                .HasConversion(
                    v => v.ToString(),
                    v => new CommunicationType(v));
            
            // Indexes
            builder.HasIndex(x => new { x.GroupId, x.PersonId })
                .IsUnique();  // Can't register for same group twice
            builder.HasIndex(x => x.RecordStatus); 
        }
    }
}
