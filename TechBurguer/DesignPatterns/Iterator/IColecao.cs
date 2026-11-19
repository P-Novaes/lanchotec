namespace TechBurguer.DesignPatterns.Iterator
{
    /// <summary>Coleção que sabe criar um iterador para si mesma (o "Aggregate" do padrão).</summary>
    public interface IColecao<T>
    {
        IIterator<T> CreateIterator();
    }
}
