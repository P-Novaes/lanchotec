using TechBurguer.DesignPatterns.Iterator;
using TechBurguer.DesignPatterns.Prototype;
using TechBurguer.Models;

namespace TechBurguer.Tests;

public class PrototypeTests
{
    private static LanchePrototype NovoMolde() => new LanchePrototype
    {
        Id = 1,
        Nome = "X-Burguer",
        PrecoBase = 18.00m,
        Ingredientes = new List<string> { "Pão brioche", "Hambúrguer 150g", "Queijo prato" }
    };

    [Fact]
    public void Clonar_gera_outro_objeto_com_os_mesmos_valores()
    {
        var molde = NovoMolde();

        var clone = molde.Clonar();

        Assert.NotSame(molde, clone);
        Assert.Equal("X-Burguer", clone.Nome);
        Assert.Equal(18.00m, clone.PrecoBase);
        Assert.Equal(molde.Ingredientes, clone.Ingredientes);
    }

    [Fact]
    public void Clonar_faz_copia_profunda_da_lista_de_ingredientes()
    {
        var molde = NovoMolde();

        var clone = molde.Clonar();
        clone.Ingredientes.Add("Bacon crocante");

        Assert.NotSame(molde.Ingredientes, clone.Ingredientes);
        Assert.DoesNotContain("Bacon crocante", molde.Ingredientes);
        Assert.Equal(3, molde.Ingredientes.Count);
        Assert.Equal(4, clone.Ingredientes.Count);
    }

    [Fact]
    public void Alterar_nome_e_preco_do_clone_nao_altera_o_molde()
    {
        var molde = NovoMolde();

        var clone = molde.Clonar();
        clone.Nome = "X-Bacon";
        clone.PrecoBase = 23.00m;

        Assert.Equal("X-Burguer", molde.Nome);
        Assert.Equal(18.00m, molde.PrecoBase);
    }

    [Fact]
    public void Catalogo_cria_XSalada_e_XBacon_clonando_o_XBurguer_sem_alterar_o_molde()
    {
        var produtos = new List<Produto>();
        var iterator = new Catalogo().CreateIterator();
        while (iterator.HasNext())
            produtos.Add(iterator.Next());

        var xBurguer = Assert.IsType<LanchePrototype>(produtos.First(p => p.Nome == "X-Burguer"));
        var xSalada = Assert.IsType<LanchePrototype>(produtos.First(p => p.Nome == "X-Salada"));
        var xBacon = Assert.IsType<LanchePrototype>(produtos.First(p => p.Nome == "X-Bacon"));

        Assert.Equal(3, xBurguer.Ingredientes.Count);
        Assert.Equal(18.00m, xBurguer.PrecoBase);
        Assert.Contains("Alface", xSalada.Ingredientes);
        Assert.DoesNotContain("Alface", xBurguer.Ingredientes);
        Assert.Contains("Bacon crocante", xBacon.Ingredientes);
        Assert.DoesNotContain("Bacon crocante", xBurguer.Ingredientes);
    }
}
