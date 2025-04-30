using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models;

public class OrderItem
{
    public int Id { get; set; }

    [Required]
    public int OrderId { get; set; }

    [Required(ErrorMessage = "Выберите товар.")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Укажите количество.")]
    [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше 0.")]
    public int Quantity { get; set; }

    public Domain.Entities.Product Product { get; set; }
}