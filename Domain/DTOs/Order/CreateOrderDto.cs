using Domain.DTOs.OrderItem;

namespace Domain.DTOs.Order;

public class CreateOrderDto
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public List<CreateOrderItemDto> Items { get; set; }
    public decimal TotalAmount { get; set; }
}