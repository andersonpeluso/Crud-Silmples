using FluentValidation;
using MiniCatalogo.Application.DTOs;

namespace MiniCatalogo.Application.Validation;

public sealed class CategoriaCreateValidator : AbstractValidator<CategoriaCreateRequest>
{
    public CategoriaCreateValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome da categoria é obrigatório.")
            .MaximumLength(50).WithMessage("Nome da categoria deve ter no máximo 50 caracteres.");
    }
}

public sealed class ProdutoCreateValidator : AbstractValidator<ProdutoCreateRequest>
{
    public ProdutoCreateValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome do produto é obrigatório.")
            .MaximumLength(100).WithMessage("Nome do produto deve ter no máximo 100 caracteres.");

        RuleFor(x => x.PrecoUnitario)
            .GreaterThanOrEqualTo(0).WithMessage("Preço unitário não pode ser negativo.");

        RuleFor(x => x.CategoriaId)
            .NotEqual(Guid.Empty).WithMessage("CategoriaId inválido.");
    }
}
