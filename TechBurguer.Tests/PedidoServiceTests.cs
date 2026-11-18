using TechBurguer.DesignPatterns.Iterator;
using TechBurguer.DesignPatterns.Prototype;
using TechBurguer.Models;
using TechBurguer.Services;

namespace TechBurguer.Tests;

public class PedidoServiceTests
{
    private const int XBurguer = 1;
    private const int Refrigerante = 5;

    private static PedidoService NovoServico() => new PedidoService(new Catalogo());

    private static List<string> Descricoes(PedidoFilaItem pedido)
    {
        var lista = new List<string>();
        var iterator = pedido.Itens.CreateIterator();
        while (iterator.HasNext())
            lista.Add(iterator.Next().Descricao);
        return lista;
    }

    [Fact]
    public void Pedido_do_exemplo_XBurguer_queijo_bacon_e_refrigerante_fecha_em_31_reais()
    {
        var pedido = NovoServico().Montar(new PedidoViewModel
        {
            NomeCliente = "Pedro",
            PrototipoId = XBurguer,
            Adicionais = new List<string> { "queijo", "bacon" },
            BebidaId = Refrigerante
        });

        Assert.Equal(31.00m, pedido.ValorTotal);
        Assert.Equal("X-Burguer", pedido.LancheBase);
        Assert.Equal("Queijo Extra, Bacon Crocante", pedido.Complementos);
        Assert.Equal("Refrigerante", pedido.Bebida);
    }

    [Fact]
    public void Queijo_bacon_e_ovo_sem_bebida_somam_27_50()
    {
        var pedido = NovoServico().Montar(new PedidoViewModel
        {
            PrototipoId = XBurguer,
            Adicionais = new List<string> { "queijo", "bacon", "ovo" }
        });

        Assert.Equal(27.50m, pedido.ValorTotal);
        Assert.Equal("Sem bebida", pedido.Bebida);
    }

    [Fact]
    public void Itens_do_recibo_seguem_a_ordem_das_camadas_e_somam_o_valor_total()
    {
        var pedido = NovoServico().Montar(new PedidoViewModel
        {
            PrototipoId = XBurguer,
            Adicionais = new List<string> { "queijo", "bacon" },
            BebidaId = Refrigerante
        });

        Assert.Equal(new[] { "X-Burguer", "+ Queijo Extra", "+ Bacon Crocante", "Refrigerante" }, Descricoes(pedido));
        Assert.Equal(pedido.ValorTotal, pedido.Itens.Total);
    }

    [Fact]
    public void Ordem_dos_adicionais_segue_o_catalogo_e_nao_a_ordem_enviada()
    {
        var pedido = NovoServico().Montar(new PedidoViewModel
        {
            PrototipoId = XBurguer,
            Adicionais = new List<string> { "ovo", "queijo" }
        });

        Assert.Equal(new[] { "X-Burguer", "+ Queijo Extra", "+ Ovo Frito" }, Descricoes(pedido));
    }

    [Fact]
    public void Clonar_como_XBacon_custa_mais_e_nao_altera_o_molde_do_catalogo()
    {
        var servico = NovoServico();

        var pedido = servico.Montar(new PedidoViewModel { PrototipoId = XBurguer, ClonarComoXBacon = true });

        Assert.Equal("X-Bacon (Clonado de X-Burguer)", pedido.LancheBase);
        Assert.Equal(23.00m, pedido.ValorTotal);

        var molde = Assert.IsType<LanchePrototype>(servico.ObterProduto(XBurguer));
        Assert.Equal("X-Burguer", molde.Nome);
        Assert.Equal(18.00m, molde.PrecoBase);
        Assert.Equal(3, molde.Ingredientes.Count);
        Assert.DoesNotContain("Bacon crocante", molde.Ingredientes);
    }

    [Fact]
    public void Clonar_como_XBacon_so_vale_para_o_XBurguer()
    {
        var pedido = NovoServico().Montar(new PedidoViewModel { PrototipoId = 2, ClonarComoXBacon = true });

        Assert.Equal("X-Salada", pedido.LancheBase);
        Assert.Equal(20.00m, pedido.ValorTotal);
    }

    [Fact]
    public void Adicional_desconhecido_e_ignorado()
    {
        var pedido = NovoServico().Montar(new PedidoViewModel
        {
            PrototipoId = XBurguer,
            Adicionais = new List<string> { "foguete" }
        });

        Assert.Equal(18.00m, pedido.ValorTotal);
        Assert.Equal("Nenhum adicional", pedido.Complementos);
    }

    [Fact]
    public void Bebida_so_e_aceita_se_existir_no_catalogo_e_for_bebida()
    {
        var servico = NovoServico();

        var comLanche = servico.Montar(new PedidoViewModel { PrototipoId = XBurguer, BebidaId = XBurguer });
        var inexistente = servico.Montar(new PedidoViewModel { PrototipoId = XBurguer, BebidaId = 999 });

        Assert.Equal("Sem bebida", comLanche.Bebida);
        Assert.Equal(18.00m, comLanche.ValorTotal);
        Assert.Equal("Sem bebida", inexistente.Bebida);
        Assert.Equal(18.00m, inexistente.ValorTotal);
    }

    [Fact]
    public void Prototipo_inexistente_ou_que_nao_e_lanche_cai_no_XBurguer()
    {
        var servico = NovoServico();

        Assert.Equal("X-Burguer", servico.Montar(new PedidoViewModel { PrototipoId = 99 }).LancheBase);
        Assert.Equal("X-Burguer", servico.Montar(new PedidoViewModel { PrototipoId = 4 }).LancheBase); // Batata Frita
    }

    [Fact]
    public void Nome_em_branco_vira_Cliente()
    {
        var pedido = NovoServico().Montar(new PedidoViewModel { NomeCliente = "   ", PrototipoId = XBurguer });

        Assert.Equal("Cliente", pedido.NomeCliente);
    }

    [Fact]
    public void Fila_comeca_com_dois_pedidos_de_exemplo_com_totais_calculados()
    {
        var fila = NovoServico().Fila;

        Assert.Equal(2, fila.Count);
        Assert.Equal(101, fila[0].NumeroSenha);
        Assert.Equal(32.00m, fila[0].ValorTotal);   // X-Bacon 23 + Queijo 3 + Refrigerante 6
        Assert.Equal(102, fila[1].NumeroSenha);
        Assert.Equal(30.50m, fila[1].ValorTotal);   // X-Salada 20 + Ovo 2,50 + Suco 8
    }

    [Fact]
    public void Criar_pedido_gera_senhas_em_sequencia_e_coloca_o_mais_novo_no_topo_da_fila()
    {
        var servico = NovoServico();

        var primeiro = servico.CriarPedido(new PedidoViewModel { NomeCliente = "Ana", PrototipoId = XBurguer });
        var segundo = servico.CriarPedido(new PedidoViewModel { NomeCliente = "Beto", PrototipoId = XBurguer });

        Assert.Equal(103, primeiro.NumeroSenha);
        Assert.Equal(104, segundo.NumeroSenha);
        Assert.Equal(4, servico.Fila.Count);
        Assert.Equal(104, servico.Fila[0].NumeroSenha);
        Assert.Same(primeiro, servico.BuscarPorSenha(103));
        Assert.Null(servico.BuscarPorSenha(9999));
    }

    [Fact]
    public void Lista_de_bebidas_vem_do_catalogo_pelo_iterator()
    {
        var nomes = NovoServico().ListarPorCategoria(Categorias.Bebida).Select(b => b.Nome).ToList();

        Assert.Equal(new[] { "Refrigerante", "Suco Natural", "Milk-shake" }, nomes);
    }
}
