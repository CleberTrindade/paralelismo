using Paralelismo.Contas.Domain.Entities;

namespace Paralelismo.Contas.Domain.Interfaces;

public interface IContaClienteRepository
{
	Task<IEnumerable<ContaCliente>> ObterContaClientes();
}
