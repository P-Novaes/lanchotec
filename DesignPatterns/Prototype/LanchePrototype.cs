using System.Collections.Generic;
using lanchotec.Models;

namespace lanchotec.DesignPatterns.Prototype
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
