namespace lanchotec.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal PrecoBase { get; set; }
        public string Categoria { get; set; } = "Lanche";
        public string Icone { get; set; } = "🍔";
    }
}
