using Paralelismo.Contas.Domain.Entities;

namespace Paralelismo.Contas.Domain.Interfaces;

public interface IContaClienteService
{
	Task<string[]> ConsolidarContas();
	Task<string[]> ConsolidarContas(CancellationToken cancellationToken);


}
