using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using lanchotec.DesignPatterns.Iterator;
using lanchotec.Models;

namespace lanchotec.Controllers
{
    public class CardapioController : Controller
    {
        public IActionResult Index()
        {
            ICatalogo catalogo = new Catalogo();
            IIterator<Produto> iterator = catalogo.CreateIterator();

            var produtos = new List<Produto>();
            while (iterator.HasNext())
            {
                produtos.Add(iterator.Next());
            }

            return View(produtos);
        }
    }
}
