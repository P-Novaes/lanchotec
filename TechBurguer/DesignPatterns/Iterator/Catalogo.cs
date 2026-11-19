using TechBurguer.DesignPatterns.Decorator;
using TechBurguer.DesignPatterns.Prototype;
using TechBurguer.Models;

namespace TechBurguer.DesignPatterns.Iterator
{
    public class Catalogo : ICatalogo
    {
        private readonly List<Produto> _produtos;

        public Catalogo()
        {
            // PROTOTYPE: o X-Burguer é o molde; os demais lanches nascem de clones dele.
            var xBurgerProto = new LanchePrototype
            {
                Id = 1,
                Nome = "X-Burguer",
                Descricao = "Pão brioche, Hambúrguer 150g, Queijo prato",
                PrecoBase = 18.00m,
                Categoria = Categorias.Lanche,
                Icone = "🍔",
                Ingredientes = new List<string> { "Pão brioche", "Hambúrguer 150g", "Queijo prato" }
            };

            var xSaladaProto = xBurgerProto.Clonar();
            xSaladaProto.Id = 2;
            xSaladaProto.Nome = "X-Salada";
            xSaladaProto.Descricao = "Pão brioche, Hambúrguer 150g, Queijo prato, Alface, Tomate";
            xSaladaProto.PrecoBase = 20.00m;
            xSaladaProto.Icone = "🥗";
            xSaladaProto.Ingredientes.AddRange(new[] { "Alface", "Tomate" });

            var xBaconProto = xBurgerProto.Clonar();
            xBaconProto.Id = 3;
            xBaconProto.Nome = "X-Bacon";
            xBaconProto.Descricao = "Pão brioche, Hambúrguer 150g, Queijo prato, Bacon crocante";
            xBaconProto.PrecoBase = 23.00m;
            xBaconProto.Icone = "🥓";
            xBaconProto.Ingredientes.Add("Bacon crocante");

            _produtos = new List<Produto>
            {
                xBurgerProto,
                xSaladaProto,
                xBaconProto,
                new Produto
                {
                    Id = 4,
                    Nome = "Batata Frita",
                    Descricao = "Batata palito, Sal, Páprica",
                    PrecoBase = BatataDecorator.PrecoAdicional,
                    Categoria = Categorias.Acompanhamento,
                    Icone = "🍟"
                },
                new Produto
                {
                    Id = 5,
                    Nome = "Refrigerante",
                    Descricao = "Lata 350ml",
                    PrecoBase = 6.00m,
                    Categoria = Categorias.Bebida,
                    Icone = "🥤"
                },
                new Produto
                {
                    Id = 6,
                    Nome = "Suco Natural",
                    Descricao = "Laranja 300ml",
                    PrecoBase = 8.00m,
                    Categoria = Categorias.Bebida,
                    Icone = "🍊"
                },
                new Produto
                {
                    Id = 7,
                    Nome = "Milk-shake",
                    Descricao = "Chocolate 400ml",
                    PrecoBase = 14.00m,
                    Categoria = Categorias.Bebida,
                    Icone = "🍨"
                }
            };
        }

        public IIterator<Produto> CreateIterator()
        {
            return new CatalogoIterator(_produtos);
        }
    }
}
