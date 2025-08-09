using Microsoft.AspNetCore.Mvc;
using MiniCatalogo.Application.DTOs;
using MiniCatalogo.Application.Services;

namespace MiniCatalogo.Api.Controllers;

[ApiController]
[Route("categorias")]
public sealed class CategoriasController(ICategoriaService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaResponse>>> Get() => Ok(await service.GetAllAsync());

    [HttpPost]
    public async Task<ActionResult<CategoriaResponse>> Post([FromBody] CategoriaCreateRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }
}
