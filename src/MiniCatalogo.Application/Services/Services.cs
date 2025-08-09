using MiniCatalogo.Application.DTOs;
using MiniCatalogo.Domain.Abstractions;
using MiniCatalogo.Domain.Entities;

namespace MiniCatalogo.Application.Services;

public interface ICategoriaService
{
    Task<CategoriaResponse> CreateAsync(CategoriaCreateRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<CategoriaResponse>> GetAllAsync(CancellationToken ct = default);
}

public interface IProdutoService
{
    Task<ProdutoResponse> CreateAsync(ProdutoCreateRequest request, CancellationToken ct = default);
    Task<PagedResult<ProdutoResponse>> GetByCategoriaPagedAsync(Guid categoriaId, int page, int size, CancellationToken ct = default);
}

public sealed class CategoriaService(ICategoriaRepository repo)
    : ICategoriaService
{
    public async Task<CategoriaResponse> CreateAsync(CategoriaCreateRequest request, CancellationToken ct = default)
    {
        var entity = new Categoria(request.Nome);
        await repo.AddAsync(entity, ct);
        return new CategoriaResponse(entity.Id, entity.Nome);
    }

    public async Task<IReadOnlyList<CategoriaResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var data = await repo.GetAllAsync(ct);
        return data.Select(c => new CategoriaResponse(c.Id, c.Nome)).ToList();
    }
}

public sealed class ProdutoService(IProdutoRepository repo, ICategoriaRepository catRepo)
    : IProdutoService
{
    public async Task<ProdutoResponse> CreateAsync(ProdutoCreateRequest request, CancellationToken ct = default)
    {
        // existência de categoria
        var cat = await catRepo.GetByIdAsync(request.CategoriaId, ct);
        if (cat is null) throw new ValidationException(["Categoria não encontrada."]);

        // unicidade por categoria (case-insensitive)
        var nomeNorm = Normalize(request.Nome);
        var exists = await repo.ExistsByNomeInCategoriaAsync(request.CategoriaId, nomeNorm, ct);
        if (exists) throw new ValidationException(["Já existe um produto com esse nome nesta categoria."]);

        var entity = new Produto(request.Nome, request.PrecoUnitario, request.CategoriaId);
        await repo.AddAsync(entity, ct);
        return new ProdutoResponse(entity.Id, entity.Nome, entity.PrecoUnitario, entity.CategoriaId);
    }

    public async Task<PagedResult<ProdutoResponse>> GetByCategoriaPagedAsync(Guid categoriaId, int page, int size, CancellationToken ct = default)
    {
        var (items, total) = await repo.GetByCategoriaPagedAsync(categoriaId, page, size, ct);
        var mapped = items.Select(p => new ProdutoResponse(p.Id, p.Nome, p.PrecoUnitario, p.CategoriaId)).ToList();
        return new PagedResult<ProdutoResponse>(mapped, total, page, size);
    }

    private static string Normalize(string s) => (s ?? string.Empty).Trim().ToUpperInvariant();
}

public sealed class ValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }
    public ValidationException(IEnumerable<string> errors) : base("Validation error")
        => Errors = errors.ToList();
}
