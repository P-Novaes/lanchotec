using TechBurguer.Models;

namespace TechBurguer.DesignPatterns.Iterator
{
    public class ItensPedidoIterator : IIterator<ItemPedido>
    {
        private readonly List<ItemPedido> _itens;
        private int _posicao = 0;

        public ItensPedidoIterator(List<ItemPedido> itens)
        {
            _itens = itens;
        }

        public bool HasNext() => _posicao < _itens.Count;

        public ItemPedido Next()
        {
            if (!HasNext())
                throw new InvalidOperationException("Não há mais itens no pedido.");

            return _itens[_posicao++];
        }

        public void Reset() => _posicao = 0;
    }
}
