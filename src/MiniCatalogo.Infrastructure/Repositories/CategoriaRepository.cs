using MiniCatalogo.Domain.Abstractions;
using MiniCatalogo.Domain.Entities;
using MiniCatalogo.Infrastructure.Persistence;

namespace MiniCatalogo.Infrastructure.Repositories;

public sealed class CategoriaRepository(InMemoryDataStore store) : ICategoriaRepository
{
    public Task AddAsync(Categoria entity, CancellationToken ct = default)
    {
        lock (store.SyncRoot) store.Categorias.Add(entity);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Categoria>> GetAllAsync(CancellationToken ct = default)
    {
        lock (store.SyncRoot) return Task.FromResult((IReadOnlyList<Categoria>)store.Categorias.ToList());
    }

    public Task<Categoria?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        lock (store.SyncRoot) return Task.FromResult(store.Categorias.FirstOrDefault(c => c.Id == id));
    }
}
