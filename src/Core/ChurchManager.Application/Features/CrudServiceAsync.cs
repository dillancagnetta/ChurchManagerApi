using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Types;

namespace ChurchManager.Application.Features
{
    public class CrudServiceAsync<TEntity, TViewDto, TEditDto> : ICrudServiceAsync<TEntity, TViewDto, TEditDto>
        where TViewDto : class 
        where TEditDto : class 
        where TEntity : class, IAggregateRoot<int>
    {
        protected readonly IGenericDbRepository<TEntity> Repository;
        private readonly IMapper _mapper;

        public CrudServiceAsync(IGenericDbRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<TViewDto>> ListAsync(CancellationToken ct = default)
        {
            var entities = await Repository.ListAsync(ct);
            return _mapper.Map<IEnumerable<TViewDto>>(entities);
        }

        public virtual async Task<TViewDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await Repository.GetByIdAsync(id, ct);
            return _mapper.Map<TViewDto>(entity);
        }

        public virtual async Task<TViewDto> AddAsync(TEditDto tDto, CancellationToken ct = default)
        {
            var entity = _mapper.Map<TEntity>(tDto);
            
            // return the view model mapped from entity
            return _mapper.Map<TViewDto>(await Repository.AddAsync(entity, ct));
        }

        public async Task<TViewDto> AddAsync(TEntity entity, CancellationToken ct = default)
        {
            var added = await Repository.AddAsync(entity, ct);
            var tDto = _mapper.Map<TViewDto>(added);
            return tDto;
        }

        public virtual async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await Repository.GetByIdAsync(id, ct) ?? throw new ArgumentNullException("id", $"Entity with id: {id} not found");
            await Repository.DeleteAsync(entity, ct);
        }

        public virtual async Task<TViewDto> UpdateAsync(TEditDto tDto, CancellationToken ct = default)
        {
            var entity = _mapper.Map<TEntity>(tDto);
            await Repository.UpdateAsync(entity, ct);
            
            // return the view model mapped from entity
            return _mapper.Map<TViewDto>(entity);
        }
    }
}
