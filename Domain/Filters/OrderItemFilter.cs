namespace Domain.Filters;

public class OrderItemFilters
{
    public int? OrderId { get; set; }
    public int? ProductId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}