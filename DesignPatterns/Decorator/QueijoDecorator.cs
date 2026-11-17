namespace lanchotec.DesignPatterns.Decorator
{
    public class QueijoDecorator : LancheDecorator
    {
        public QueijoDecorator(ILanche lanche) : base(lanche) { }
        public override string GetDescricao() => $"{_lanche.GetDescricao()} + Queijo Extra";
        public override decimal GetPreco() => _lanche.GetPreco() + 3.00m;
    }
}
