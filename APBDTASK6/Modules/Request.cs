using System.ComponentModel.DataAnnotations;

namespace WebApplication1;

public class Request
{
    [Required] public int IdProduct { get; set; }
    [Required] public int IdWarehouse { get; set; }
    [Required] public int Amount { get; set; }
    [Required] public DateTime CreatedAt { get; set; }
}