namespace lanchotec.DesignPatterns.Decorator
{
    public abstract class LancheDecorator : ILanche
    {
        protected readonly ILanche _lanche;

        protected LancheDecorator(ILanche lanche)
        {
            _lanche = lanche;
        }

        public virtual string GetDescricao() => _lanche.GetDescricao();
        public virtual decimal GetPreco() => _lanche.GetPreco();
    }
}
