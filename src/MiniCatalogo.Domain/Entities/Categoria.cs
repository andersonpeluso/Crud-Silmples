namespace MiniCatalogo.Domain.Entities;

public sealed class Categoria
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }

    private Categoria() { }

    public Categoria(string nome)
    {
        Id = Guid.NewGuid();
        Nome = nome?.Trim() ?? string.Empty;
    }

    public void Renomear(string nome)
    {
        Nome = nome?.Trim() ?? string.Empty;
    }
}
