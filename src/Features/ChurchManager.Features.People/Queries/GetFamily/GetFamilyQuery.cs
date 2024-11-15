using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Specifications;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MassTransit.Initializers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.People.Queries.GetFamily
{
    public record GetFamilyQuery(int FamilyId, bool IncludePeople = false) : IRequest<ApiResponse>;

    public class GetFamilyHandler : IRequestHandler<GetFamilyQuery, ApiResponse>
    {
        private readonly IGenericDbRepository<Family> _dbRepository;
        private readonly IGenericDbRepository<Person> _repository;

        public GetFamilyHandler(IGenericDbRepository<Family> dbRepository,
            IGenericDbRepository<Person> repository)
        {
            _dbRepository = dbRepository;
            _repository = repository;
        }

        public async Task<ApiResponse> Handle(GetFamilyQuery query, CancellationToken ct)
        {
            /*var qry = _dbRepository.Queryable();
            
            if (query.IncludePeople)
            {
                qry = qry.Include(x => x.FamilyMembers);
                qry = qry.Include(x => x.Address);
            }
            
            var family = await qry.FirstOrDefaultAsync(x => x.Id == query.FamilyId, ct)
                    ?.Select(x =>
                    new {
                        x.Name,
                        x.Language,
                        FamilyMembers = x.FamilyMembers.Select(f => new {f.BirthDate, f.AgeClassification, f.Gender, f.BaptismStatus})
                            
                    })
                         ?? throw new ArgumentNullException(nameof(query.FamilyId));
            
            var index = family.Name.IndexOf("Family", StringComparison.InvariantCultureIgnoreCase);
            var vm = new FamilyViewModel
            {
                Id = family.Id,
                Name = index > -1 ? family.Name.Remove(index - 1, 7) : family.Name,
                City = family.Address?.City,
                Country = family.Address?.Country,
                Street = family.Address?.Street,
                PostalCode = family.Address?.PostalCode,
                Province = family.Address?.Province,
            };

            if (query.IncludePeople)
            {
                vm.FamilyMembers = family.FamilyMembers?.Select(x => new PersonViewModelBasic
                {
                   PersonId   = x.Id,
                   Gender = x.Gender,
                   FirstName = x.FullName.FirstName,
                   LastName = x.FullName.LastName,
                   AgeClassification = x.AgeClassification,
                   Age = x.BirthDate.Age,
                   PhotoUrl = x.PhotoUrl
                }).ToList();
            }*/

            var spec = new FamilyWithMembersSpecification(query.FamilyId, query.IncludePeople);

            var vm = await _dbRepository.GetBySpecAsync<FamilyViewModel>(spec, ct);

            return new ApiResponse(vm);
        }
    }
}
 
