using System.Collections.Generic;

namespace lanchotec.Models
{
    public class Lanche : Produto
    {
        public List<string> Ingredientes { get; set; } = new List<string>();
    }
}
