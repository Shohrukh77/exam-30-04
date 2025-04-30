using System.Net;
using AutoMapper;
using Domain.DTOs.Customer;
using Domain.DTOs.Order;
using Domain.DTOs.OrderItem;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class OrderService(ApplicationDbContext context, IMapper  mapper) : IOrderService
{
    public async Task<Response<List<GetOrderDto>>> GetAllAsync(OrderFilter filter)
    {
        var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);
        var query = context.Orders.AsQueryable();
        
        if (filter.CustomerId != null)
            query = query.Where(x => x.CustomerId <= filter.CustomerId);

        var mapped = mapper.Map<List<GetOrderDto>>(query.ToList());
        var totalRecords = mapped.Count;

        var data = mapped
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        return new PagedResponse<List<GetOrderDto>>(data, validFilter.PageNumber, validFilter.PageSize, totalRecords);

    }
    
    public async Task<Response<GetOrderDto>> CreateAsync(CreateOrderDto request)
    {
        var order = mapper.Map<Order>(request);
        await context.Orders.AddAsync(order);
        var result = await context.SaveChangesAsync();
        var data = mapper.Map<GetOrderDto>(order);

        return result == 0 ?
        new Response<GetOrderDto>(HttpStatusCode.BadRequest, "order not added!")
        : new Response<GetOrderDto>(data);
    }



    public async Task<Response<GetOrderDto>> GetAsync(int id)
    {
        var o = await context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        var data = mapper.Map<GetOrderDto>(o);
        return new Response<GetOrderDto>(data);
    }

}