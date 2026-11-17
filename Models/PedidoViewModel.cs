using System.Collections.Generic;

namespace lanchotec.Models
{
    public class PedidoViewModel
    {
        public string NomeCliente { get; set; } = string.Empty;
        public int PrototipoId { get; set; } = 1;
        public bool ClonarComoXBacon { get; set; }

        // Adicionais Decorator
        public bool AdicionarQueijo { get; set; }
        public bool AdicionarBacon { get; set; }
        public bool AdicionarOvo { get; set; }

        // Bebidas (Seção 3 do protótipo)
        public string BebidaSelecionada { get; set; } = "Nenhuma";

        // Lista de itens para a tela de montar (via Iterator)
        public List<Produto> CardapioDisponivel { get; set; } = new List<Produto>();
    }

    public class PedidoFilaItem
    {
        public int NumeroSenha { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string LancheBase { get; set; } = string.Empty;
        public string Complementos { get; set; } = string.Empty;
        public string Bebida { get; set; } = string.Empty;
        public string DescricaoCompleta { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public string Status { get; set; } = "Em preparo";
        public DateTime HoraPedido { get; set; } = DateTime.Now;
    }
}
