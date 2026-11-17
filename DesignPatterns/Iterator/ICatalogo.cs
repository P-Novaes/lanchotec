using lanchotec.Models;

namespace lanchotec.DesignPatterns.Iterator
{
    public interface ICatalogo
    {
        IIterator<Produto> CreateIterator();
    }
}
