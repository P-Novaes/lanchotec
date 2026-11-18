using TechBurguer.DesignPatterns.Iterator;
using TechBurguer.Models;

namespace TechBurguer.Tests;

public class IteratorTests
{
    [Fact]
    public void Percorre_todos_os_produtos_na_ordem_do_catalogo()
    {
        var nomes = new List<string>();
        IIterator<Produto> iterator = new Catalogo().CreateIterator();

        while (iterator.HasNext())
            nomes.Add(iterator.Next().Nome);

        Assert.Equal(
            new[] { "X-Burguer", "X-Salada", "X-Bacon", "Batata Frita", "Refrigerante", "Suco Natural", "Milk-shake" },
            nomes);
    }

    [Fact]
    public void Reset_volta_ao_primeiro_produto()
    {
        var iterator = new Catalogo().CreateIterator();
        var primeiro = iterator.Next();
        iterator.Next();

        iterator.Reset();

        Assert.True(iterator.HasNext());
        Assert.Same(primeiro, iterator.Next());
    }

    [Fact]
    public void Next_depois_do_fim_lanca_excecao()
    {
        var iterator = new CatalogoIterator(new List<Produto> { new Produto { Id = 1, Nome = "Único" } });
        iterator.Next();

        Assert.False(iterator.HasNext());
        Assert.Throws<InvalidOperationException>(() => iterator.Next());
    }

    [Fact]
    public void Colecao_vazia_nao_tem_proximo()
    {
        var iterator = new CatalogoIterator(new List<Produto>());

        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void Itens_do_pedido_sao_percorridos_na_ordem_e_somam_o_total()
    {
        var itens = new ItensPedido();
        itens.Adicionar(new ItemPedido("X-Burguer", 18.00m));
        itens.Adicionar(new ItemPedido("+ Queijo Extra", 3.00m));
        itens.Adicionar(new ItemPedido("Refrigerante", 6.00m));

        var descricoes = new List<string>();
        var iterator = itens.CreateIterator();
        while (iterator.HasNext())
            descricoes.Add(iterator.Next().Descricao);

        Assert.Equal(new[] { "X-Burguer", "+ Queijo Extra", "Refrigerante" }, descricoes);
        Assert.Equal(3, itens.Quantidade);
        Assert.Equal(27.00m, itens.Total);
    }

    [Fact]
    public void Iterador_de_itens_vazio_nao_tem_proximo()
    {
        Assert.False(new ItensPedido().CreateIterator().HasNext());
    }
}
