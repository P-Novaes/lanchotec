namespace lanchotec.DesignPatterns.Decorator
{
    public class OvoDecorator : LancheDecorator
    {
        public OvoDecorator(ILanche lanche) : base(lanche) { }
        public override string GetDescricao() => $"{_lanche.GetDescricao()} + Ovo Frito";
        public override decimal GetPreco() => _lanche.GetPreco() + 2.50m;
    }
}
