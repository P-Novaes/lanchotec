using System.Collections.Generic;

namespace TechBurguer.Models
{
    public class Lanche : Produto
    {
        public List<string> Ingredientes { get; set; } = new List<string>();
    }
}
