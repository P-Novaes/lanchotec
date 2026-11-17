using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using lanchotec.DesignPatterns.Decorator;
using lanchotec.DesignPatterns.Iterator;
using lanchotec.DesignPatterns.Prototype;
using lanchotec.Models;

namespace lanchotec.Controllers
{
    public class PedidoController : Controller
    {
        private static readonly Dictionary<int, LanchePrototype> _prototiposBase = new()
        {
            {
                1, new LanchePrototype
                {
                    Id = 1,
                    Nome = "X-Burguer",
                    Descricao = "Pão brioche, Hambúrguer 150g, Queijo prato",
                    PrecoBase = 18.00m,
                    Icone = "🍔",
                    Ingredientes = new List<string> { "Pão brioche", "Hambúrguer 150g", "Queijo prato" }
                }
            },
            {
                2, new LanchePrototype
                {
                    Id = 2,
                    Nome = "X-Salada",
                    Descricao = "Pão brioche, Hambúrguer 150g, Queijo prato, Alface, Tomate",
                    PrecoBase = 20.00m,
                    Icone = "🥗",
                    Ingredientes = new List<string> { "Pão brioche", "Hambúrguer 150g", "Queijo prato", "Alface", "Tomate" }
                }
            },
            {
                3, new LanchePrototype
                {
                    Id = 3,
                    Nome = "X-Bacon",
                    Descricao = "Pão brioche, Hambúrguer 150g, Queijo prato, Bacon crocante",
                    PrecoBase = 22.50m,
                    Icone = "🥓",
                    Ingredientes = new List<string> { "Pão brioche", "Hambúrguer 150g", "Queijo prato", "Bacon crocante" }
                }
            },
            {
                4, new LanchePrototype
                {
                    Id = 4,
                    Nome = "Batata Frita",
                    Descricao = "Batata palito, Sal, Páprica",
                    PrecoBase = 12.00m,
                    Icone = "🍟",
                    Ingredientes = new List<string> { "Batata palito", "Sal", "Páprica" }
                }
            }
        };

        private static readonly List<PedidoFilaItem> _filaPedidos = new()
        {
            new PedidoFilaItem
            {
                NumeroSenha = 101,
                NomeCliente = "Lucas",
                LancheBase = "X-Bacon",
                Complementos = "Queijo Extra",
                Bebida = "Coca-Cola Lata",
                DescricaoCompleta = "X-Bacon + Queijo Extra + Coca-Cola Lata",
                ValorTotal = 31.50m,
                Status = "Pronto para retirada",
                HoraPedido = DateTime.Now.AddMinutes(-8)
            },
            new PedidoFilaItem
            {
                NumeroSenha = 102,
                NomeCliente = "Mariana",
                LancheBase = "X-Salada",
                Complementos = "Ovo Frito",
                Bebida = "Suco Natural",
                DescricaoCompleta = "X-Salada + Ovo Frito + Suco Natural",
                ValorTotal = 30.50m,
                Status = "Em preparo",
                HoraPedido = DateTime.Now.AddMinutes(-3)
            }
        };

        private static int _contadorSenha = 103;

        private List<Produto> CarregarCardapioViaIterator()
        {
            ICatalogo catalogo = new Catalogo();
            IIterator<Produto> iterator = catalogo.CreateIterator();
            var lista = new List<Produto>();
            while (iterator.HasNext())
            {
                lista.Add(iterator.Next());
            }
            return lista;
        }

        [HttpGet]
        public IActionResult Montar(int? id)
        {
            var model = new PedidoViewModel
            {
                PrototipoId = (id.HasValue && _prototiposBase.ContainsKey(id.Value)) ? id.Value : 1,
                CardapioDisponivel = CarregarCardapioViaIterator()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Montar(PedidoViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.NomeCliente))
            {
                model.NomeCliente = "Cliente";
            }

            if (!_prototiposBase.TryGetValue(model.PrototipoId, out var prototipoSelecionado))
            {
                prototipoSelecionado = _prototiposBase[1];
            }

            // 1. PROTOTYPE: Clona profundamente o lanche selecionado sem alterar o molde original
            LanchePrototype lancheClonado = prototipoSelecionado.Clonar();

            if (model.ClonarComoXBacon && lancheClonado.Id == 1)
            {
                lancheClonado.Nome = "X-Bacon (Clonado de X-Burguer)";
                lancheClonado.PrecoBase += 4.50m;
                lancheClonado.Ingredientes.Add("Bacon crocante");
            }

            // 2. DECORATOR: Envolve o lanche clonado com adicionais dinâmicos
            ILanche lancheDecorado = new LancheBase(lancheClonado);
            var listaComplementos = new List<string>();

            if (model.AdicionarQueijo)
            {
                lancheDecorado = new QueijoDecorator(lancheDecorado);
                listaComplementos.Add("Queijo Extra");
            }

            if (model.AdicionarBacon)
            {
                lancheDecorado = new BaconDecorator(lancheDecorado);
                listaComplementos.Add("Bacon Crocante");
            }

            if (model.AdicionarOvo)
            {
                lancheDecorado = new OvoDecorator(lancheDecorado);
                listaComplementos.Add("Ovo Frito");
            }

            string bebidaTexto = "Sem bebida";
            if (!string.IsNullOrWhiteSpace(model.BebidaSelecionada) && model.BebidaSelecionada != "Nenhuma")
            {
                var partes = model.BebidaSelecionada.Split('|');
                string nomeBebida = partes[0];
                decimal precoBebida = partes.Length > 1 && decimal.TryParse(partes[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var p) ? p : 6.00m;
                lancheDecorado = new BebidaDecorator(lancheDecorado, nomeBebida, precoBebida);
                bebidaTexto = nomeBebida;
            }

            var novoPedido = new PedidoFilaItem
            {
                NumeroSenha = _contadorSenha++,
                NomeCliente = model.NomeCliente.Trim(),
                LancheBase = lancheClonado.Nome,
                Complementos = listaComplementos.Any() ? string.Join(", ", listaComplementos) : "Nenhum adicional",
                Bebida = bebidaTexto,
                DescricaoCompleta = lancheDecorado.GetDescricao(),
                ValorTotal = lancheDecorado.GetPreco(),
                Status = "Em preparo",
                HoraPedido = DateTime.Now
            };

            _filaPedidos.Insert(0, novoPedido);

            return RedirectToAction("Obrigado", new { senha = novoPedido.NumeroSenha });
        }

        [HttpGet]
        public IActionResult Obrigado(int senha)
        {
            var pedido = _filaPedidos.FirstOrDefault(p => p.NumeroSenha == senha);
            return View(pedido);
        }

        [HttpGet]
        public IActionResult Fila()
        {
            return View(_filaPedidos);
        }
    }
}
