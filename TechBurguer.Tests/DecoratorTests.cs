using TechBurguer.DesignPatterns.Decorator;
using TechBurguer.DesignPatterns.Prototype;

namespace TechBurguer.Tests;

public class DecoratorTests
{
    private static ILanche XBurguer() => new LancheBase(new LanchePrototype
    {
        Id = 1,
        Nome = "X-Burguer",
        PrecoBase = 18.00m,
        Ingredientes = new List<string> { "Pão brioche", "Hambúrguer 150g" }
    });

    [Fact]
    public void Lanche_sem_adicionais_custa_o_preco_base()
    {
        Assert.Equal(18.00m, XBurguer().GetPreco());
    }

    [Fact]
    public void Queijo_bacon_e_ovo_somam_27_50()
    {
        ILanche lanche = XBurguer();
        lanche = new QueijoDecorator(lanche);   // + 3,00
        lanche = new BaconDecorator(lanche);    // + 4,00
        lanche = new OvoDecorator(lanche);      // + 2,50

        Assert.Equal(27.50m, lanche.GetPreco());
    }

    [Fact]
    public void Descricao_acumula_as_camadas_na_ordem_em_que_foram_aplicadas()
    {
        ILanche lanche = new BaconDecorator(new QueijoDecorator(XBurguer()));

        Assert.StartsWith("X-Burguer", lanche.GetDescricao());
        Assert.EndsWith(" + Queijo Extra + Bacon Crocante", lanche.GetDescricao());
    }

    [Fact]
    public void O_mesmo_decorator_pode_ser_aplicado_duas_vezes_sem_criar_classe_nova()
    {
        ILanche lanche = new QueijoDecorator(new QueijoDecorator(XBurguer()));

        Assert.Equal(24.00m, lanche.GetPreco());
        Assert.EndsWith(" + Queijo Extra + Queijo Extra", lanche.GetDescricao());
    }

    [Fact]
    public void Bebida_entra_como_camada_final_do_pedido()
    {
        ILanche lanche = new BaconDecorator(new QueijoDecorator(XBurguer()));
        lanche = new BebidaDecorator(lanche, "Refrigerante", 6.00m);

        Assert.Equal(31.00m, lanche.GetPreco());
        Assert.EndsWith(" + Refrigerante", lanche.GetDescricao());
    }

    [Fact]
    public void Decorar_nao_altera_o_lanche_original()
    {
        var original = XBurguer();

        var decorado = new OvoDecorator(original);

        Assert.Equal(18.00m, original.GetPreco());
        Assert.Equal(20.50m, decorado.GetPreco());
    }

    [Fact]
    public void Catalogo_de_adicionais_aplica_o_decorator_correto()
    {
        var batata = CatalogoAdicionais.Obter("batata");

        Assert.NotNull(batata);
        Assert.Equal(28.00m, batata!.Aplicar(XBurguer()).GetPreco());
        Assert.Equal(4, CatalogoAdicionais.Todos.Count);
    }

    [Fact]
    public void Chave_de_adicional_desconhecida_nao_existe_no_catalogo()
    {
        Assert.Null(CatalogoAdicionais.Obter("foguete"));
    }
}
