using MiniCatalogo.Domain.Abstractions;
using MiniCatalogo.Domain.Entities;
using MiniCatalogo.Infrastructure.Persistence;

namespace MiniCatalogo.Infrastructure.Repositories;

public sealed class ProdutoRepository(InMemoryDataStore store) : IProdutoRepository
{
    public Task AddAsync(Produto entity, CancellationToken ct = default)
    {
        lock (store.SyncRoot) store.Produtos.Add(entity);
        return Task.CompletedTask;
    }

    public Task<Produto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        lock (store.SyncRoot) return Task.FromResult(store.Produtos.FirstOrDefault(p => p.Id == id));
    }

    public Task<(IReadOnlyList<Produto> Items, int Total)> GetByCategoriaPagedAsync(Guid categoriaId, int page, int size, CancellationToken ct = default)
    {
        lock (store.SyncRoot)
        {
            var query = store.Produtos.Where(p => p.CategoriaId == categoriaId);
            var total = query.Count();
            var skip = (page - 1) * size;
            var items = query.OrderBy(p => p.Nome).Skip(skip).Take(size).ToList();
            return Task.FromResult(((IReadOnlyList<Produto>)items, total));
        }
    }

    public Task<bool> ExistsByNomeInCategoriaAsync(Guid categoriaId, string nomeNormalizado, CancellationToken ct = default)
    {
        lock (store.SyncRoot)
        {
            return Task.FromResult(store.Produtos.Any(p => p.CategoriaId == categoriaId && p.Nome.Trim().ToUpperInvariant() == nomeNormalizado));
        }
    }
}
