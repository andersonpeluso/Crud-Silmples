using FluentValidation;
using FluentValidation.AspNetCore;
using MiniCatalogo.Application.Services;
using MiniCatalogo.Application.Validation;
using MiniCatalogo.Domain.Abstractions;
using MiniCatalogo.Infrastructure.Persistence;
using MiniCatalogo.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddSingleton<InMemoryDataStore>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CategoriaCreateValidator>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Middleware de tratamento de exceções de validação da aplicação
app.Use(async (ctx, next) =>
{
    try
    {
        await next();
    }
    catch (MiniCatalogo.Application.Services.ValidationException vex)
    {
        ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
        await ctx.Response.WriteAsJsonAsync(new { errors = vex.Errors });
    }
});

app.Run();
