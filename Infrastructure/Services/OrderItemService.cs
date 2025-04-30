using System.Net;
using AutoMapper;
using Domain.DTOs.OrderItem;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class OrderItemService(ApplicationDbContext context, IMapper mapper) : IOrderItemService
{
    public async Task<Response<List<GetOrderItemDto>>> GetAllAsync(OrderItemFilters filter)
    {
        var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);
        var query = context.OrderItems.AsQueryable();

        if (filter.OrderId != null)
           query = query.Where(x => x.OrderId == filter.OrderId);
        
        if (filter.ProductId != null)
           query = query.Where(x => x.ProductId == filter.ProductId);

        var mapped = mapper.Map<List<GetOrderItemDto>>(query.ToList());
        var totalRecords = mapped.Count;

        var data = mapped
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        return new PagedResponse<List<GetOrderItemDto>>(data, validFilter.PageNumber, validFilter.PageSize, totalRecords);
    }

    public async Task<Response<GetOrderItemDto>> CreateAsync(CreateOrderItemDto request)
    {
        var orderItem = mapper.Map<OrderItem>(request);
        await context.OrderItems.AddAsync(orderItem);
        var result = await context.SaveChangesAsync();

        var data = mapper.Map<GetOrderItemDto>(orderItem);
        return result == 0
            ? new Response<GetOrderItemDto>(HttpStatusCode.BadRequest, "OrderItem not added!")
            : new Response<GetOrderItemDto>(data);
    }

    public async Task<Response<GetOrderItemDto>> GetAsync(int id)
    {
        var orderItem = await context.OrderItems.FindAsync(id);
        if (orderItem == null)
            return new Response<GetOrderItemDto>(HttpStatusCode.BadRequest, "OrderItem not found");

        var data = mapper.Map<GetOrderItemDto>(orderItem);
        return new Response<GetOrderItemDto>(data);
    }


    public async Task<Response<GetOrderItemDto>> UpdateAsync(int id, UpdateOrderItemDto request)
    {
        var existing = await context.OrderItems.FindAsync(id);
        if (existing == null)
            return new Response<GetOrderItemDto>(HttpStatusCode.BadRequest, "OrderItem not found");

        existing.OrderId = request.OrderId;
        existing.ProductId = request.ProductId;
        existing.Quantity = request.Quantity;
        existing.Product.Name = request.ProductName;

        var result = await context.SaveChangesAsync();
        var data = mapper.Map<GetOrderItemDto>(existing);
        return result == 0
            ? new Response<GetOrderItemDto>(HttpStatusCode.BadRequest, "OrderItem not updated!")
            : new Response<GetOrderItemDto>(data);
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var orderItem = await context.OrderItems.FindAsync(id);
        if (orderItem == null)
            return new Response<string>(HttpStatusCode.BadRequest, "OrderItem not found");

        context.OrderItems.Remove(orderItem);
        var result = await context.SaveChangesAsync();

        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "OrderItem not deleted!")
            : new Response<string>("OrderItem deleted!");
    }
}