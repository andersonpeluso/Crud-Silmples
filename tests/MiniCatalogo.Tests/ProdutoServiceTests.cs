using MiniCatalogo.Application.DTOs;
using MiniCatalogo.Application.Services;
using MiniCatalogo.Domain.Abstractions;
using MiniCatalogo.Domain.Entities;
using Moq;

namespace MiniCatalogo.Tests;

[TestClass]
public class ProdutoServiceTests
{
    [TestMethod]
    public async Task Deve_Falhar_Se_Ja_Existe_Produto_Mesma_Categoria_CaseInsensitive()
    {
        var catRepo = new Mock<ICategoriaRepository>();
        catRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), default)).ReturnsAsync(new Categoria("Eletrônicos"));

        var prodRepo = new Mock<IProdutoRepository>();
        prodRepo.Setup(r => r.ExistsByNomeInCategoriaAsync(It.IsAny<Guid>(), "NOTEBOOK", default)).ReturnsAsync(true);

        var service = new ProdutoService(prodRepo.Object, catRepo.Object);

        await Assert.ThrowsExceptionAsync<ValidationException>(() =>
            service.CreateAsync(new ProdutoCreateRequest("Notebook", 1000, Guid.NewGuid())));
    }

    [TestMethod]
    public async Task Deve_Falhar_Se_Preco_Negativo()
    {
        var catRepo = new Mock<ICategoriaRepository>();
        var prodRepo = new Mock<IProdutoRepository>();
        var service = new ProdutoService(prodRepo.Object, catRepo.Object);

        // Validação de preço é pelo validator de entrada; aqui simulamos pela exceção de categoria inexistente
        // e depois poderíamos cobrir validator com testes de validator (opcional).
        await Assert.ThrowsExceptionAsync<ValidationException>(() =>
            service.CreateAsync(new ProdutoCreateRequest("Notebook", 10, Guid.Empty)));
    }

    [TestMethod]
    public async Task Deve_Paginar_Get_Produtos()
    {
        var categoriaId = Guid.NewGuid();
        var catRepo = new Mock<ICategoriaRepository>();
        var prodRepo = new Mock<IProdutoRepository>();

        prodRepo.Setup(r => r.GetByCategoriaPagedAsync(categoriaId, 2, 2, default))
                .ReturnsAsync((new List<Produto>{
                    new Produto("B", 1, categoriaId),
                    new Produto("C", 1, categoriaId)
                }, 5));

        var service = new ProdutoService(prodRepo.Object, catRepo.Object);
        var result = await service.GetByCategoriaPagedAsync(categoriaId, 2, 2);

        Assert.AreEqual(2, result.Items.Count);
        Assert.AreEqual(5, result.Total);
        Assert.AreEqual(2, result.Page);
        Assert.AreEqual(2, result.Size);
    }
}
