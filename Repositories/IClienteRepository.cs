using ClienteCrudNetMySql.Models;

namespace ClienteCrudNetMySql.Repositories;

public interface IClienteRepository
{
    Task<List<Cliente>> ObtenerTodosAsync(string? busqueda = null);
    Task<Cliente?> ObtenerPorIdAsync(int id);
    Task CrearAsync(Cliente cliente);
    Task ActualizarAsync(Cliente cliente);
    Task EliminarAsync(int id);
}
