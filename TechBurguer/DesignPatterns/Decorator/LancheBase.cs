using TechBurguer.Models;

namespace TechBurguer.DesignPatterns.Decorator
{
    public class LancheBase : ILanche
    {
        private readonly Lanche _lanche;

        public LancheBase(Lanche lanche)
        {
            _lanche = lanche;
        }

        public string GetDescricao()
        {
            return $"{_lanche.Nome} ({string.Join(", ", _lanche.Ingredientes)})";
        }

        public decimal GetPreco() => _lanche.PrecoBase;
    }
}
