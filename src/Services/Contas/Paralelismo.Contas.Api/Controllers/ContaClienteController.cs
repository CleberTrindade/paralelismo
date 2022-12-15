using Microsoft.AspNetCore.Mvc;
using Paralelismo.Contas.Domain.Interfaces;

namespace Paralelismo.Contas.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ContaClienteController : ControllerBase
	{		
		private readonly IContaClienteService _contaClienteService;
		private readonly ILogger<ContaClienteController> _logger;

		public ContaClienteController(ILogger<ContaClienteController> logger, 
									  IContaClienteService contaClienteService)
		{
			_logger = logger;
			_contaClienteService = contaClienteService;
		}

		[HttpGet("/ConsolidarContasComCancelationToken")]
		public async Task<IEnumerable<string>> ConsolidarContas(CancellationToken cancellationToken)
		{
			var result = new List<string>();
			try
			{
				_logger.LogInformation("Iniciando o processamento");


				var retorno = await _contaClienteService.ConsolidarContas(cancellationToken);			

				result = retorno.ToList();

				_logger.LogInformation("Finalizando o processamento");
				
			}
			catch (OperationCanceledException)
			{

				_logger.LogInformation("Cancelando o processamento");
			}

			return result;			
		}

		[HttpGet("/ConsolidarContasSemCancelationToken")]
		public async Task<IEnumerable<string>> ConsolidarContasSemCancelationToken()
		{
			var result = new List<string>();
			try
			{
				_logger.LogInformation("Iniciando o processamento");

				var tt = await _contaClienteService.ConsolidarContas();

				result = tt.ToList();

				_logger.LogInformation("Finalizando o processamento");

			}
			catch (OperationCanceledException)
			{

				_logger.LogInformation("Cancelando o processamento");
			}

			return result;
		}
	}
}