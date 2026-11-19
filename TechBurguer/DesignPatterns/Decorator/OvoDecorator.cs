namespace TechBurguer.DesignPatterns.Decorator
{
    public class OvoDecorator : LancheDecorator
    {
        public const string NomeAdicional = "Ovo Frito";
        public const decimal PrecoAdicional = 2.50m;

        public OvoDecorator(ILanche lanche) : base(lanche) { }
        public override string GetDescricao() => $"{_lanche.GetDescricao()} + {NomeAdicional}";
        public override decimal GetPreco() => _lanche.GetPreco() + PrecoAdicional;
    }
}
