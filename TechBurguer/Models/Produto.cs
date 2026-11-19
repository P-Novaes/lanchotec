namespace TechBurguer.Models
{
    public static class Categorias
    {
        public const string Lanche = "Lanche";
        public const string Acompanhamento = "Acompanhamento";
        public const string Bebida = "Bebida";
    }

    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal PrecoBase { get; set; }
        public string Categoria { get; set; } = Categorias.Lanche;
        public string Icone { get; set; } = "🍔";
    }
}
