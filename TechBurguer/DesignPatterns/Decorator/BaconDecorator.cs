namespace TechBurguer.DesignPatterns.Decorator
{
    public class BaconDecorator : LancheDecorator
    {
        public const string NomeAdicional = "Bacon Crocante";
        public const decimal PrecoAdicional = 4.00m;

        public BaconDecorator(ILanche lanche) : base(lanche) { }
        public override string GetDescricao() => $"{_lanche.GetDescricao()} + {NomeAdicional}";
        public override decimal GetPreco() => _lanche.GetPreco() + PrecoAdicional;
    }
}
