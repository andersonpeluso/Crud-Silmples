using MiniCatalogo.Application.DTOs;
using MiniCatalogo.Application.Services;
using MiniCatalogo.Domain.Abstractions;
using Moq;

namespace MiniCatalogo.Tests;

[TestClass]
public class CategoriaServiceTests
{
    [TestMethod]
    public async Task Deve_Criar_Categoria()
    {
        var repo = new Mock<ICategoriaRepository>();
        var service = new CategoriaService(repo.Object);

        var resposta = await service.CreateAsync(new CategoriaCreateRequest("Games"));
        Assert.IsFalse(resposta.Id == Guid.Empty);
        Assert.AreEqual("Games", resposta.Nome);
    }
}
