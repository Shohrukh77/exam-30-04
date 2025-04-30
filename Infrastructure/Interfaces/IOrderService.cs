using Domain.DTOs.Order;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IOrderService
{
    Task<Response<List<GetOrderDto>>> GetAllAsync(OrderFilter  filter);
    Task<Response<GetOrderDto>> CreateAsync(CreateOrderDto request);
    Task<Response<GetOrderDto>> GetAsync(int id);
}