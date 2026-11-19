using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TechBurguer.DesignPatterns.Decorator;
using TechBurguer.DesignPatterns.Iterator;

namespace TechBurguer.Models
{
    public class PedidoViewModel
    {
        public string NomeCliente { get; set; } = string.Empty;

        // Prototype: qual lanche do cardápio será clonado
        public int PrototipoId { get; set; } = 1;
        public bool ClonarComoXBacon { get; set; }

        // Decorator: chaves dos adicionais marcados na tela ("queijo", "bacon", "ovo", "batata")
        public List<string> Adicionais { get; set; } = new List<string>();

        // Bebida escolhida (Id do produto no catálogo; 0 = sem bebida)
        public int BebidaId { get; set; }

        // ---- Dados que o controller preenche para desenhar a tela (vindos do Iterator / catálogo).
        // Nunca devem vir do navegador, por isso ficam fora do binding e da validação. ----
        [BindNever, ValidateNever] public List<Produto> CardapioDisponivel { get; set; } = new List<Produto>();   // lanches
        [BindNever, ValidateNever] public List<Produto> Bebidas { get; set; } = new List<Produto>();
        [BindNever, ValidateNever] public IReadOnlyList<AdicionalInfo> AdicionaisDisponiveis { get; set; } = CatalogoAdicionais.Todos;
        [BindNever, ValidateNever] public int ClonePrototipoId { get; set; }
        [BindNever, ValidateNever] public decimal CloneAcrescimo { get; set; }
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

        // Itens do pedido final, percorridos pelo Iterator na tela de recibo.
        public ItensPedido Itens { get; set; } = new ItensPedido();
    }
}
