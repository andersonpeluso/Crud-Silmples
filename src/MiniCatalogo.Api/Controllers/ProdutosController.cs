using Microsoft.AspNetCore.Mvc;
using MiniCatalogo.Application.DTOs;
using MiniCatalogo.Application.Services;

namespace MiniCatalogo.Api.Controllers;

[ApiController]
[Route("produtos")]
public sealed class ProdutosController(IProdutoService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ProdutoResponse>>> Get([FromQuery] Guid categoriaId, [FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        if (categoriaId == Guid.Empty) return BadRequest(new { errors = new[] { "categoriaId é obrigatório." } });
        if (page <= 0 || size <= 0) return BadRequest(new { errors = new[] { "page e size devem ser positivos." } });
        var result = await service.GetByCategoriaPagedAsync(categoriaId, page, size);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoResponse>> Post([FromBody] ProdutoCreateRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { categoriaId = created.CategoriaId, page = 1, size = 10 }, created);
    }
}
