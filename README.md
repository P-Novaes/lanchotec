# Tech Burguer

Lanchonete em **ASP.NET Core MVC** (.NET 10) feita para demonstrar três padrões de projeto (GoF):
**Prototype**, **Decorator** e **Iterator**.

## Como rodar

```bash
dotnet run --project TechBurguer      # abre em http://localhost:5267
dotnet test                           # roda os testes (precisa de internet na 1ª vez, para baixar o xUnit)
```

No Visual Studio: abra `TechBurguer.sln` e execute o projeto `TechBurguer`.

## Onde cada padrão está no código

| Padrão | Pasta / arquivos | O que faz no sistema |
|---|---|---|
| **Iterator** | `DesignPatterns/Iterator` — `IIterator<T>`, `IColecao<T>`, `ICatalogo`, `Catalogo`, `CatalogoIterator`, `ItensPedido`, `ItensPedidoIterator` | Percorre o catálogo (Cardápio, lista de lanches e de bebidas) e os itens do pedido final (recibo) |
| **Prototype** | `DesignPatterns/Prototype` — `ILanchePrototype`, `LanchePrototype.Clonar()` | `Catalogo` cria X-Salada e X-Bacon clonando o X-Burguer; cada pedido clona o lanche e nunca altera o molde |
| **Decorator** | `DesignPatterns/Decorator` — `ILanche`, `LancheBase`, `LancheDecorator`, `Queijo/Bacon/Ovo/Batata/BebidaDecorator`, `CatalogoAdicionais` | Cada adicional embrulha o lanche e soma descrição e preço |

`Services/PedidoService.cs` junta os três em sequência: Iterator (lê o catálogo) → Prototype (clona) → Decorator (adicionais e bebida) → Iterator (itens do recibo).

## Estrutura (MVC)

```
TechBurguer.sln
TechBurguer/                 projeto web
  Controllers/               Home, Cardapio, Pedido
  Models/                    Produto, ItemPedido, PedidoViewModel (+ PedidoFilaItem)
  Services/                  PedidoService (regra do pedido, testável sem MVC)
  DesignPatterns/            Prototype, Decorator, Iterator
  Views/                     Home, Cardapio, Pedido (Montar, Obrigado, Fila), Shared
  wwwroot/                   css e imagens
TechBurguer.Tests/           testes xUnit (Prototype, Decorator, Iterator, PedidoService)
```

## Roteiro da demonstração

1. **Início** → "Ver Cardápio": a lista vem do **Iterator** (`CardapioController`).
2. Clique no **X-Burguer** (R$ 18,00) para abrir "Montar".
3. Seção 1 (**Prototype**): marque "Clonar X-Burguer como novo X-Bacon" e mostre o nome mudando no resumo.
4. Seção 2 (**Decorator**): marque Queijo Extra e Bacon Crocante; veja o preço somando a cada camada.
5. Seção 3: escolha Refrigerante. Sem o clone, o total é **R$ 31,00** (18 + 3 + 4 + 6).
6. **Finalizar pedido**: o recibo lista os itens percorridos pelo **Iterator** e mostra o total; depois o pedido aparece na **Fila**.

## Preços (um só lugar para cada)

- Lanches e bebidas: `DesignPatterns/Iterator/Catalogo.cs`
- Adicionais: constante `PrecoAdicional` de cada `*Decorator.cs`
- Acréscimo do clone "X-Bacon": `PedidoService.AcrescimoXBacon`
