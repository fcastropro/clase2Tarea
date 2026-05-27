using System.ComponentModel.DataAnnotations;
using System.Net;

namespace ClienteCrudNetMySql.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La cédula es obligatoria")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "La cédula debe tener 10 dígitos")]
    [RegularExpression("^[0-9]+$", ErrorMessage = "La cédula solo debe contener números")]
    public string Cedula { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 80 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 80 caracteres")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
    [StringLength(120)]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [StringLength(15, MinimumLength = 7, ErrorMessage = "El teléfono debe tener entre 7 y 15 dígitos")]
    [RegularExpression("^[0-9]+$", ErrorMessage = "El teléfono solo debe contener números")]
    public string Telefono { get; set; } = string.Empty;

    [StringLength(150, ErrorMessage = "La dirección no debe superar 150 caracteres")]
    public string? Direccion { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    public void Sanitizar()
    {
        Cedula = Limpiar(Cedula);
        Nombre = Limpiar(Nombre);
        Apellido = Limpiar(Apellido);
        Correo = Limpiar(Correo).ToLowerInvariant();
        Telefono = Limpiar(Telefono);
        Direccion = string.IsNullOrWhiteSpace(Direccion) ? null : Limpiar(Direccion);
    }

    private static string Limpiar(string? valor)
    {
        return WebUtility.HtmlEncode((valor ?? string.Empty).Trim());
    }
}
