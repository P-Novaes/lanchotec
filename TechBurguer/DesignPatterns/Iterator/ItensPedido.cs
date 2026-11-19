using TechBurguer.Models;

namespace TechBurguer.DesignPatterns.Iterator
{
    /// <summary>Segunda coleção do sistema: as linhas do pedido final, também percorridas por um Iterator.</summary>
    public class ItensPedido : IColecao<ItemPedido>
    {
        private readonly List<ItemPedido> _itens = new List<ItemPedido>();

        public int Quantidade => _itens.Count;
        public decimal Total => _itens.Sum(i => i.Valor);

        public void Adicionar(ItemPedido item) => _itens.Add(item);

        public IIterator<ItemPedido> CreateIterator() => new ItensPedidoIterator(_itens);
    }
}
