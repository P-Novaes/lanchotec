namespace lanchotec.DesignPatterns.Decorator
{
    public class BaconDecorator : LancheDecorator
    {
        public BaconDecorator(ILanche lanche) : base(lanche) { }
        public override string GetDescricao() => $"{_lanche.GetDescricao()} + Bacon Crocante";
        public override decimal GetPreco() => _lanche.GetPreco() + 4.50m;
    }
}
