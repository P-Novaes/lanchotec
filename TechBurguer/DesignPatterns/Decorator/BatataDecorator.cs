namespace TechBurguer.DesignPatterns.Decorator
{
    public class BatataDecorator : LancheDecorator
    {
        public const string NomeAdicional = "Batata Frita";
        public const decimal PrecoAdicional = 10.00m;

        public BatataDecorator(ILanche lanche) : base(lanche) { }
        public override string GetDescricao() => $"{_lanche.GetDescricao()} + {NomeAdicional}";
        public override decimal GetPreco() => _lanche.GetPreco() + PrecoAdicional;
    }
}
