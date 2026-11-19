using System.Collections.Generic;
using TechBurguer.Models;

namespace TechBurguer.DesignPatterns.Prototype
{
    public class LanchePrototype : Lanche, ILanchePrototype
    {
        public LanchePrototype Clonar()
        {
            var clone = (LanchePrototype)this.MemberwiseClone();
            clone.Ingredientes = new List<string>(this.Ingredientes);
            return clone;
        }
    }
}
