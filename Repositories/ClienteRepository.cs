using ClienteCrudNetMySql.Models;
using MySqlConnector;

namespace ClienteCrudNetMySql.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly string _connectionString;

    public ClienteRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No existe la cadena de conexión DefaultConnection.");
    }

    public async Task<List<Cliente>> ObtenerTodosAsync(string? busqueda = null)
    {
        var clientes = new List<Cliente>();
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"SELECT id, cedula, nombre, apellido, correo, telefono, direccion, fecha_registro
                    FROM clientes
                    WHERE @busqueda IS NULL
                       OR cedula LIKE CONCAT('%', @busqueda, '%')
                       OR nombre LIKE CONCAT('%', @busqueda, '%')
                       OR apellido LIKE CONCAT('%', @busqueda, '%')
                       OR correo LIKE CONCAT('%', @busqueda, '%')
                    ORDER BY id DESC";

        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@busqueda", string.IsNullOrWhiteSpace(busqueda) ? DBNull.Value : busqueda.Trim());

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            clientes.Add(MapearCliente(reader));
        }

        return clientes;
    }

    public async Task<Cliente?> ObtenerPorIdAsync(int id)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"SELECT id, cedula, nombre, apellido, correo, telefono, direccion, fecha_registro
                    FROM clientes WHERE id = @id LIMIT 1";
        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? MapearCliente(reader) : null;
    }

    public async Task CrearAsync(Cliente cliente)
    {
        cliente.Sanitizar();
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"INSERT INTO clientes (cedula, nombre, apellido, correo, telefono, direccion)
                    VALUES (@cedula, @nombre, @apellido, @correo, @telefono, @direccion)";
        await using var command = new MySqlCommand(sql, connection);
        AgregarParametros(command, cliente);
        await command.ExecuteNonQueryAsync();
    }

    public async Task ActualizarAsync(Cliente cliente)
    {
        cliente.Sanitizar();
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"UPDATE clientes
                    SET cedula = @cedula, nombre = @nombre, apellido = @apellido,
                        correo = @correo, telefono = @telefono, direccion = @direccion
                    WHERE id = @id";
        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", cliente.Id);
        AgregarParametros(command, cliente);
        await command.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new MySqlCommand("DELETE FROM clientes WHERE id = @id", connection);
        command.Parameters.AddWithValue("@id", id);
        await command.ExecuteNonQueryAsync();
    }

    private static void AgregarParametros(MySqlCommand command, Cliente cliente)
    {
        command.Parameters.AddWithValue("@cedula", cliente.Cedula);
        command.Parameters.AddWithValue("@nombre", cliente.Nombre);
        command.Parameters.AddWithValue("@apellido", cliente.Apellido);
        command.Parameters.AddWithValue("@correo", cliente.Correo);
        command.Parameters.AddWithValue("@telefono", cliente.Telefono);
        command.Parameters.AddWithValue("@direccion", cliente.Direccion ?? (object)DBNull.Value);
    }

    private static Cliente MapearCliente(MySqlDataReader reader)
    {
        return new Cliente
        {
            Id = reader.GetInt32("id"),
            Cedula = reader.GetString("cedula"),
            Nombre = reader.GetString("nombre"),
            Apellido = reader.GetString("apellido"),
            Correo = reader.GetString("correo"),
            Telefono = reader.GetString("telefono"),
            Direccion = reader.IsDBNull(reader.GetOrdinal("direccion")) ? null : reader.GetString("direccion"),
            FechaRegistro = reader.GetDateTime("fecha_registro")
        };
    }
}
