#!/usr/bin/env bash
set -euo pipefail

# Gere a solução e adicione projetos/referências.
# Execute a partir da PASTA RAIZ (onde este script está), por exemplo: bash scripts/setup.sh

if ! command -v dotnet >/dev/null; then
  echo "dotnet SDK não encontrado no PATH. Instale o .NET 8 SDK."
  exit 1
fi

# Cria solução (se não existir)
[ -f MiniCatalogoDDD.sln ] || dotnet new sln -n MiniCatalogoDDD

# Adiciona projetos à solução
dotnet sln MiniCatalogoDDD.sln add src/MiniCatalogo.Api/MiniCatalogo.Api.csproj
dotnet sln MiniCatalogoDDD.sln add src/MiniCatalogo.Application/MiniCatalogo.Application.csproj
dotnet sln MiniCatalogoDDD.sln add src/MiniCatalogo.Domain/MiniCatalogo.Domain.csproj
dotnet sln MiniCatalogoDDD.sln add src/MiniCatalogo.Infrastructure/MiniCatalogo.Infrastructure.csproj
dotnet sln MiniCatalogoDDD.sln add tests/MiniCatalogo.Tests/MiniCatalogo.Tests.csproj

# Referências entre projetos
dotnet add src/MiniCatalogo.Api/MiniCatalogo.Api.csproj reference src/MiniCatalogo.Application/MiniCatalogo.Application.csproj
dotnet add src/MiniCatalogo.Application/MiniCatalogo.Application.csproj reference src/MiniCatalogo.Domain/MiniCatalogo.Domain.csproj
dotnet add src/MiniCatalogo.Infrastructure/MiniCatalogo.Infrastructure.csproj reference src/MiniCatalogo.Domain/MiniCatalogo.Domain.csproj

# Pacotes
dotnet add src/MiniCatalogo.Application/MiniCatalogo.Application.csproj package FluentValidation --version 11.9.2
dotnet add src/MiniCatalogo.Application/MiniCatalogo.Application.csproj package FluentValidation.AspNetCore --version 11.3.0

dotnet add tests/MiniCatalogo.Tests/MiniCatalogo.Tests.csproj package Moq --version 4.20.72
dotnet add tests/MiniCatalogo.Tests/MiniCatalogo.Tests.csproj package MSTest.TestAdapter --version 3.1.1
dotnet add tests/MiniCatalogo.Tests/MiniCatalogo.Tests.csproj package MSTest.TestFramework --version 3.1.1

echo "Pronto! Agora rode: dotnet build"
