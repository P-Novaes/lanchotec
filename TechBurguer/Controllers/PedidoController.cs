using Microsoft.AspNetCore.Mvc;
using TechBurguer.Models;
using TechBurguer.Services;

namespace TechBurguer.Controllers
{
    public class PedidoController : Controller
    {
        private readonly PedidoService _pedidos;

        public PedidoController(PedidoService pedidos)
        {
            _pedidos = pedidos;
        }

        [HttpGet]
        public IActionResult Montar(int? id)
        {
            var lanches = _pedidos.ListarPorCategoria(Categorias.Lanche);

            var model = new PedidoViewModel
            {
                PrototipoId = (id.HasValue && lanches.Any(l => l.Id == id.Value)) ? id.Value : PedidoService.PrototipoClonavelId,
                CardapioDisponivel = lanches,
                Bebidas = _pedidos.ListarPorCategoria(Categorias.Bebida),
                ClonePrototipoId = PedidoService.PrototipoClonavelId,
                CloneAcrescimo = PedidoService.AcrescimoXBacon
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Montar(PedidoViewModel model)
        {
            var pedido = _pedidos.CriarPedido(model);
            return RedirectToAction("Obrigado", new { senha = pedido.NumeroSenha });
        }

        [HttpGet]
        public IActionResult Obrigado(int senha)
        {
            return View(_pedidos.BuscarPorSenha(senha));
        }

        [HttpGet]
        public IActionResult Fila()
        {
            return View(_pedidos.Fila);
        }
    }
}
