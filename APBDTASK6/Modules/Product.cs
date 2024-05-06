using System.ComponentModel.DataAnnotations;

namespace WebApplication1;

public class Product
{
    [Required] public int IdProduct { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Description { get; set; }
    [Required] public double Price { get; set; }
}