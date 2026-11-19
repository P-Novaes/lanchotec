namespace TechBurguer.Models
{
    /// <summary>Uma linha do pedido final (ex.: "X-Burguer R$ 18,00" ou "+ Queijo Extra R$ 3,00").</summary>
    public record ItemPedido(string Descricao, decimal Valor);
}
