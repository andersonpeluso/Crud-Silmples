# Mini Catálogo — DDD (.NET 8)

## Requisitos
- .NET 8 SDK

## Como rodar
```bash
# na raiz da solução
dotnet build
cd src/MiniCatalogo.Api
dotnet run
```

Acesse o Swagger em: `https://localhost:5001/swagger` (ou porta exibida no console)

## Endpoints

- `GET /categorias`
- `POST /categorias`
- `GET /produtos?categoriaId=<guid>&page=<int>&size=<int>`
- `POST /produtos`

## Decisões

- DDD **enxuto** com separação por camadas
- Persistência **InMemory** para foco nas regras
- **FluentValidation** para mensagens de erro claras (400)
- Testes **MSTest** cobrindo unicidade, preço negativo (via validação) e paginação

## Scripts úteis (opcional)
Veja `scripts/setup.sh` para gerar a solução e adicionar referências com `dotnet`.
