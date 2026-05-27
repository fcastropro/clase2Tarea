using ClienteCrudNetMySql.Models;
using ClienteCrudNetMySql.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ClienteCrudNetMySql.Controllers;

public class ClientesController : Controller
{
    private readonly IClienteRepository _clienteRepository;

    public ClientesController(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? busqueda)
    {
        ViewBag.Busqueda = busqueda;
        var clientes = await _clienteRepository.ObtenerTodosAsync(busqueda);
        return View(clientes);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);
        return cliente is null ? NotFound() : View(cliente);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Cliente());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Cliente cliente)
    {
        cliente.Sanitizar();
        if (!ModelState.IsValid) return View(cliente);

        try
        {
            await _clienteRepository.CrearAsync(cliente);
            TempData["Mensaje"] = "Cliente registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "No se pudo registrar el cliente: " + ex.Message);
            return View(cliente);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);
        return cliente is null ? NotFound() : View(cliente);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Cliente cliente)
    {
        if (id != cliente.Id) return BadRequest();
        cliente.Sanitizar();
        if (!ModelState.IsValid) return View(cliente);

        try
        {
            await _clienteRepository.ActualizarAsync(cliente);
            TempData["Mensaje"] = "Cliente actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "No se pudo actualizar el cliente: " + ex.Message);
            return View(cliente);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);
        return cliente is null ? NotFound() : View(cliente);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _clienteRepository.EliminarAsync(id);
        TempData["Mensaje"] = "Cliente eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
