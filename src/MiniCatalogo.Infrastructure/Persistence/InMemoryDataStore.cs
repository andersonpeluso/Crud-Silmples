using MiniCatalogo.Domain.Entities;

namespace MiniCatalogo.Infrastructure.Persistence;

public sealed class InMemoryDataStore
{
    public List<Categoria> Categorias { get; } = new();
    public List<Produto> Produtos { get; } = new();
    public object SyncRoot { get; } = new();
}
