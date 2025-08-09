namespace MiniCatalogo.Domain.Abstractions;

public interface IRepository<T>
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
}

public interface ICategoriaRepository : IRepository<MiniCatalogo.Domain.Entities.Categoria>
{
    Task<IReadOnlyList<MiniCatalogo.Domain.Entities.Categoria>> GetAllAsync(CancellationToken ct = default);
}

public interface IProdutoRepository : IRepository<MiniCatalogo.Domain.Entities.Produto>
{
    Task<(IReadOnlyList<MiniCatalogo.Domain.Entities.Produto> Items, int Total)> GetByCategoriaPagedAsync(
        Guid categoriaId, int page, int size, CancellationToken ct = default);

    Task<bool> ExistsByNomeInCategoriaAsync(Guid categoriaId, string nomeNormalizado, CancellationToken ct = default);
}
