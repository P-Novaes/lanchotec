using TechBurguer.DesignPatterns.Decorator;
using TechBurguer.DesignPatterns.Iterator;
using TechBurguer.DesignPatterns.Prototype;
using TechBurguer.Models;

namespace TechBurguer.Services
{
    /// <summary>
    /// Regra de negócio do pedido, separada do controller para poder ser testada sem MVC.
    /// Usa os três padrões em sequência: Iterator (lê o catálogo) -> Prototype (clona o lanche)
    /// -> Decorator (soma adicionais e bebida) -> Iterator (percorre os itens do pedido final).
    /// </summary>
    public class PedidoService
    {
        /// <summary>Lanche que pode ser clonado como "X-Bacon" na tela de montar (o X-Burguer).</summary>
        public const int PrototipoClonavelId = 1;
        public const decimal AcrescimoXBacon = 5.00m;

        private readonly ICatalogo _catalogo;
        private readonly List<PedidoFilaItem> _fila = new List<PedidoFilaItem>();
        private readonly object _trava = new object();
        private int _contadorSenha = 103;

        public PedidoService(ICatalogo catalogo)
        {
            _catalogo = catalogo;

            // Dois pedidos de exemplo na fila, criados pelo mesmo fluxo real de montagem.
            var lucas = Montar(new PedidoViewModel
            {
                NomeCliente = "Lucas", PrototipoId = 3, Adicionais = new List<string> { "queijo" }, BebidaId = 5
            });
            lucas.NumeroSenha = 101;
            lucas.Status = "Pronto para retirada";
            lucas.HoraPedido = DateTime.Now.AddMinutes(-8);

            var mariana = Montar(new PedidoViewModel
            {
                NomeCliente = "Mariana", PrototipoId = 2, Adicionais = new List<string> { "ovo" }, BebidaId = 6
            });
            mariana.NumeroSenha = 102;
            mariana.HoraPedido = DateTime.Now.AddMinutes(-3);

            _fila.Add(lucas);
            _fila.Add(mariana);
        }

        public IReadOnlyList<PedidoFilaItem> Fila
        {
            get { lock (_trava) { return _fila.ToList(); } }
        }

        public PedidoFilaItem? BuscarPorSenha(int senha)
        {
            lock (_trava) { return _fila.FirstOrDefault(p => p.NumeroSenha == senha); }
        }

        // ---------- ITERATOR: leitura do catálogo ----------

        public List<Produto> ListarPorCategoria(string categoria)
        {
            var lista = new List<Produto>();
            IIterator<Produto> iterator = _catalogo.CreateIterator();
            while (iterator.HasNext())
            {
                var produto = iterator.Next();
                if (produto.Categoria == categoria)
                    lista.Add(produto);
            }
            return lista;
        }

        public Produto? ObterProduto(int id)
        {
            IIterator<Produto> iterator = _catalogo.CreateIterator();
            while (iterator.HasNext())
            {
                var produto = iterator.Next();
                if (produto.Id == id)
                    return produto;
            }
            return null;
        }

        // ---------- Criação do pedido ----------

        /// <summary>Monta o pedido e o coloca no topo da fila, com uma senha nova.</summary>
        public PedidoFilaItem CriarPedido(PedidoViewModel model)
        {
            var pedido = Montar(model);
            lock (_trava)
            {
                pedido.NumeroSenha = _contadorSenha++;
                _fila.Insert(0, pedido);
            }
            return pedido;
        }

        /// <summary>Monta o pedido (sem colocá-lo na fila): Prototype + Decorator + itens do recibo.</summary>
        public PedidoFilaItem Montar(PedidoViewModel model)
        {
            var nomeCliente = string.IsNullOrWhiteSpace(model.NomeCliente) ? "Cliente" : model.NomeCliente.Trim();

            // 1. PROTOTYPE: clona o lanche escolhido; o molde do catálogo nunca é alterado.
            var molde = ObterProduto(model.PrototipoId) as LanchePrototype
                        ?? (LanchePrototype)ObterProduto(PrototipoClonavelId)!;
            LanchePrototype lanche = molde.Clonar();

            if (model.ClonarComoXBacon && molde.Id == PrototipoClonavelId)
            {
                lanche.Nome = "X-Bacon (Clonado de X-Burguer)";
                lanche.PrecoBase += AcrescimoXBacon;
                lanche.Ingredientes.Add("Bacon crocante");
            }

            var itens = new ItensPedido();
            itens.Adicionar(new ItemPedido(lanche.Nome, lanche.PrecoBase));

            // 2. DECORATOR: cada adicional marcado embrulha o lanche com mais uma camada.
            ILanche decorado = new LancheBase(lanche);
            var complementos = new List<string>();
            var marcados = model.Adicionais ?? new List<string>();

            foreach (var adicional in CatalogoAdicionais.Todos)
            {
                if (!marcados.Contains(adicional.Chave))
                    continue;

                decorado = adicional.Aplicar(decorado);
                complementos.Add(adicional.Nome);
                itens.Adicionar(new ItemPedido("+ " + adicional.Nome, adicional.Preco));
            }

            // A bebida também é uma camada: o preço vem do catálogo, nunca da tela.
            var bebidaTexto = "Sem bebida";
            var bebida = model.BebidaId > 0 ? ObterProduto(model.BebidaId) : null;
            if (bebida != null && bebida.Categoria == Categorias.Bebida)
            {
                decorado = new BebidaDecorator(decorado, bebida.Nome, bebida.PrecoBase);
                bebidaTexto = bebida.Nome;
                itens.Adicionar(new ItemPedido(bebida.Nome, bebida.PrecoBase));
            }

            return new PedidoFilaItem
            {
                NomeCliente = nomeCliente,
                LancheBase = lanche.Nome,
                Complementos = complementos.Any() ? string.Join(", ", complementos) : "Nenhum adicional",
                Bebida = bebidaTexto,
                DescricaoCompleta = decorado.GetDescricao(),
                ValorTotal = decorado.GetPreco(),
                Itens = itens,
                Status = "Em preparo",
                HoraPedido = DateTime.Now
            };
        }
    }
}
