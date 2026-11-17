namespace lanchotec.DesignPatterns.Decorator
{
    public class BebidaDecorator : LancheDecorator
    {
        private readonly string _nomeBebida;
        private readonly decimal _precoBebida;

        public BebidaDecorator(ILanche lanche, string nomeBebida, decimal precoBebida) : base(lanche)
        {
            _nomeBebida = nomeBebida;
            _precoBebida = precoBebida;
        }

        public override string GetDescricao() => $"{_lanche.GetDescricao()} + {_nomeBebida}";
        public override decimal GetPreco() => _lanche.GetPreco() + _precoBebida;
    }
}
