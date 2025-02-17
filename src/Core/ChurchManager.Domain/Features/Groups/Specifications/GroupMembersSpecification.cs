﻿using Ardalis.Specification;
using ChurchManager.Domain.Common;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class GroupMembersSpecification : Specification<Group>
    {
        public GroupMembersSpecification(int groupId, RecordStatus recordStatus)
        {
            Query.AsNoTracking();

            Query.Where(x => x.Id == groupId);

            if (recordStatus is not null && !recordStatus.Value.IsNullOrEmpty())
            {
                Query.Where(x => x.Members.Any(x => x.RecordStatus == recordStatus));
            }

            Query.OrderBy(x => x.Name);

            Query.Include("Members.Person");
            Query.Include("Members.GroupRole");
        }
    }
}
