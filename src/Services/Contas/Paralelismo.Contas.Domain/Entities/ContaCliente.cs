namespace Paralelismo.Contas.Domain.Entities;

public  class ContaCliente
{
	public string? NomeCliente { get; set; }
	public List<Movimento>? Movimentacoes { get; set; }
	public decimal Investimento { get; set; }
}
