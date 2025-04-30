namespace Domain.Filters;

public class OrderFilter
{
    public int? CustomerId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}