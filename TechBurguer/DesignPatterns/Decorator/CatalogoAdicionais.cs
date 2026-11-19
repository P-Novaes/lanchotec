namespace TechBurguer.DesignPatterns.Decorator
{
    /// <summary>Descreve um adicional: como aparece na tela e qual Decorator ele aplica.</summary>
    public record AdicionalInfo(string Chave, string Nome, decimal Preco, Func<ILanche, ILanche> Aplicar);

    /// <summary>
    /// Lista única dos adicionais. Para criar um adicional novo basta escrever o Decorator
    /// e registrá-lo aqui: a tela e o cálculo do pedido passam a enxergá-lo sozinhos.
    /// </summary>
    public static class CatalogoAdicionais
    {
        public static readonly IReadOnlyList<AdicionalInfo> Todos = new List<AdicionalInfo>
        {
            new("queijo", QueijoDecorator.NomeAdicional, QueijoDecorator.PrecoAdicional, l => new QueijoDecorator(l)),
            new("bacon",  BaconDecorator.NomeAdicional,  BaconDecorator.PrecoAdicional,  l => new BaconDecorator(l)),
            new("ovo",    OvoDecorator.NomeAdicional,    OvoDecorator.PrecoAdicional,    l => new OvoDecorator(l)),
            new("batata", BatataDecorator.NomeAdicional, BatataDecorator.PrecoAdicional, l => new BatataDecorator(l)),
        };

        public static AdicionalInfo? Obter(string chave) => Todos.FirstOrDefault(a => a.Chave == chave);
    }
}
