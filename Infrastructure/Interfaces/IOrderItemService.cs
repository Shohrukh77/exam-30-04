using Domain.DTOs.OrderItem;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IOrderItemService
{
    Task<Response<List<GetOrderItemDto>>> GetAllAsync(OrderItemFilters  filters);
    Task<Response<GetOrderItemDto>> CreateAsync(CreateOrderItemDto request);
    Task<Response<GetOrderItemDto>> GetAsync(int id);
    Task<Response<GetOrderItemDto>> UpdateAsync(int id, UpdateOrderItemDto request);
    Task<Response<string>> DeleteAsync(int id);
}