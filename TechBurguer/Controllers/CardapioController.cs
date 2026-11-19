using Microsoft.AspNetCore.Mvc;
using TechBurguer.DesignPatterns.Iterator;
using TechBurguer.Models;

namespace TechBurguer.Controllers
{
    public class CardapioController : Controller
    {
        private readonly ICatalogo _catalogo;

        public CardapioController(ICatalogo catalogo)
        {
            _catalogo = catalogo;
        }

        public IActionResult Index()
        {
            // ITERATOR: o controller percorre o catálogo sem saber como ele guarda os produtos.
            IIterator<Produto> iterator = _catalogo.CreateIterator();

            var produtos = new List<Produto>();
            while (iterator.HasNext())
            {
                produtos.Add(iterator.Next());
            }

            return View(produtos);
        }
    }
}
