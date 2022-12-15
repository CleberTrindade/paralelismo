using Microsoft.Extensions.Logging;
using Paralelismo.Contas.Domain.Entities;
using Paralelismo.Contas.Domain.Interfaces;

namespace Paralelismo.Contas.Application.Services;

public class ContaClienteService : IContaClienteService
{

	private readonly IContaClienteRepository _contaClienteRepository;
	private readonly ILogger<ContaClienteService> _logger;
	public ContaClienteService(IContaClienteRepository contaClienteRepository, ILogger<ContaClienteService> logger)
	{
		_contaClienteRepository = contaClienteRepository;
		_logger = logger;
	}

	public async Task<string[]> ConsolidarContas()
	{
		return await ConsolidarContas(CancellationToken.None);
	}

	public async Task<string[]> ConsolidarContas(CancellationToken cancellationToken)
	{

		var contas = await _contaClienteRepository.ObterContaClientes();

		var tasks = contas.Select(conta =>
				Task.Factory.StartNew(() =>
				{
					cancellationToken.ThrowIfCancellationRequested();

					var resultadoConsolidacao = ConsolidarMovimentacao(conta, cancellationToken);

					cancellationToken.ThrowIfCancellationRequested();

					return resultadoConsolidacao;
				})
			);

		return await Task.WhenAll(tasks);
	}

	private string ConsolidarMovimentacao(ContaCliente conta, CancellationToken cancellationToken)
	{
		var soma = 0m;
		var cont = 0;

		foreach (var movimento in conta.Movimentacoes)
		{
			_logger.LogInformation($"Conta: {++cont}");
			cancellationToken.ThrowIfCancellationRequested();
			soma += movimento.Valor * FatorDeMultiplicacao(movimento.Data);
		}

		cancellationToken.ThrowIfCancellationRequested();
		AtualizarInvestimentos(conta);
		return $"Cliente {conta.NomeCliente} tem saldo atualizado de R${soma.ToString("#00.00")}";
	}

	private static decimal FatorDeMultiplicacao(DateTime dataMovimento)
	{
		const decimal CTE_FATOR = 1.0000000005m;

		var diasCorridosDesdeDataMovimento = (dataMovimento - new DateTime(1900, 1, 1)).Days;
		var resultado = 1m;

		for (int i = 0; i < diasCorridosDesdeDataMovimento * 2; i++)
			resultado = resultado * CTE_FATOR;

		return resultado;
	}
	private static void AtualizarInvestimentos(ContaCliente cliente)
	{
		const decimal CTE_BONIFICACAO_MOV = 1m / (10m * 5m);
		cliente.Investimento *= CTE_BONIFICACAO_MOV;
	}
}
