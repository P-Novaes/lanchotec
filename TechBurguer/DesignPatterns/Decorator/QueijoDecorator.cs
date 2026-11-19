namespace TechBurguer.DesignPatterns.Decorator
{
    public class QueijoDecorator : LancheDecorator
    {
        public const string NomeAdicional = "Queijo Extra";
        public const decimal PrecoAdicional = 3.00m;

        public QueijoDecorator(ILanche lanche) : base(lanche) { }
        public override string GetDescricao() => $"{_lanche.GetDescricao()} + {NomeAdicional}";
        public override decimal GetPreco() => _lanche.GetPreco() + PrecoAdicional;
    }
}
