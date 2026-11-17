using System;
using System.Collections.Generic;
using lanchotec.Models;

namespace lanchotec.DesignPatterns.Iterator
{
    public class CatalogoIterator : IIterator<Produto>
    {
        private readonly List<Produto> _itens;
        private int _posicao = 0;

        public CatalogoIterator(List<Produto> itens)
        {
            _itens = itens;
        }

        public bool HasNext() => _posicao < _itens.Count;

        public Produto Next()
        {
            if (!HasNext())
                throw new InvalidOperationException("Não há mais itens no catálogo.");

            return _itens[_posicao++];
        }

        public void Reset() => _posicao = 0;
    }
}
