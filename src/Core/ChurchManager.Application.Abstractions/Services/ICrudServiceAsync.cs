namespace ChurchManager.Application.Abstractions.Services
{
    public interface ICrudServiceAsync<in TEntity, TViewDto, in TEditDto>
    {
        Task<IEnumerable<TViewDto>> ListAsync(CancellationToken ct = default);
        Task<TViewDto> GetByIdAsync(int id, CancellationToken ct = default);
        Task<TViewDto> AddAsync(TEditDto tDto, CancellationToken ct = default);
        Task<TViewDto> AddAsync(TEntity entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
        //Task UpdateAsync(TEditDto entityTDto, CancellationToken ct = default);
        Task<TViewDto> UpdateAsync(TEditDto entityTDto, CancellationToken ct = default);
    }
}
