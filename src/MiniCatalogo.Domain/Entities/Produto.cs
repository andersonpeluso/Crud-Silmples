namespace MiniCatalogo.Domain.Entities;

public sealed class Produto
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public decimal PrecoUnitario { get; private set; }
    public Guid CategoriaId { get; private set; }

    private Produto() { }

    public Produto(string nome, decimal precoUnitario, Guid categoriaId)
    {
        Id = Guid.NewGuid();
        Nome = nome?.Trim() ?? string.Empty;
        PrecoUnitario = precoUnitario;
        CategoriaId = categoriaId;
    }

    public void Atualizar(string nome, decimal precoUnitario)
    {
        Nome = nome?.Trim() ?? string.Empty;
        PrecoUnitario = precoUnitario;
    }
}
