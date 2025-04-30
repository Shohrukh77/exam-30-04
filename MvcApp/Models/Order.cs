using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace MvcApp.Models;

public class Order
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Клиент обязателен.")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Дата заказа обязательна.")]
    [DataType(DataType.Date)]
    public DateTime OrderDate { get; set; }

    [Required(ErrorMessage = "Должно быть хотя бы одно наименование.")]
    [MinLength(1, ErrorMessage = "Добавьте хотя бы один товар.")]
    public List<OrderItem> Items { get; set; } = new();

    public Customer Customer { get; set; }
}