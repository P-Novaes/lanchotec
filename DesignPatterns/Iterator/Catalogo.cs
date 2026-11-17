using System.Collections.Generic;
using lanchotec.DesignPatterns.Prototype;
using lanchotec.Models;

namespace lanchotec.DesignPatterns.Iterator
{
    public class Catalogo : ICatalogo
    {
        private readonly List<Produto> _produtos;

        public Catalogo()
        {
            var xBurgerProto = new LanchePrototype
            {
                Id = 1,
                Nome = "X-Burguer",
                Descricao = "Pão brioche, Hambúrguer 150g, Queijo prato",
                PrecoBase = 18.00m,
                Categoria = "Lanche",
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
            xBaconProto.PrecoBase = 22.50m;
            xBaconProto.Icone = "🥓";
            xBaconProto.Ingredientes.Add("Bacon crocante");

            var batataProto = new LanchePrototype
            {
                Id = 4,
                Nome = "Batata Frita",
                Descricao = "Batata palito, Sal, Páprica",
                PrecoBase = 12.00m,
                Categoria = "Acompanhamento",
                Icone = "🍟",
                Ingredientes = new List<string> { "Batata palito", "Sal", "Páprica" }
            };

            _produtos = new List<Produto>
            {
                xBurgerProto,
                xSaladaProto,
                xBaconProto,
                batataProto
            };
        }

        public IIterator<Produto> CreateIterator()
        {
            return new CatalogoIterator(_produtos);
        }
    }
}
