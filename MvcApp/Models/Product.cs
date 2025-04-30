using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Название товара обязательно.")]
    [StringLength(100, ErrorMessage = "Название не может быть длиннее 100 символов.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Цена обязательна.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0.")]
    public decimal Price { get; set; }
}