namespace MiniCatalogo.Application.DTOs;

public sealed record CategoriaCreateRequest(string Nome);
public sealed record CategoriaResponse(Guid Id, string Nome);

public sealed record ProdutoCreateRequest(string Nome, decimal PrecoUnitario, Guid CategoriaId);
public sealed record ProdutoResponse(Guid Id, string Nome, decimal PrecoUnitario, Guid CategoriaId);

public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int Size);
