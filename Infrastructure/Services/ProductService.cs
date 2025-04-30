using System.Net;
using AutoMapper;
using Domain.DTOs.Product;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class ProductService(ApplicationDbContext context, IMapper mapper) : IProductService
{
    public async Task<Response<List<GetProductDto>>> GetAllAsync(ProductFilter filter)
    {
        var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);
        var query = context.Products.AsQueryable();

        if (filter.Name != null)
            query = query.Where(p => p.Name.ToLower().Contains(filter.Name.ToLower()));

        if (filter.From != null)
            query = query.Where(p => p.Price >= filter.Price);

        if (filter.To != null)
            query = query.Where(p => p.Price <= filter.Price);

        var mapped = mapper.Map<List<GetProductDto>>(query.ToList());
        var totalRecords = mapped.Count;

        var data = mapped
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        return new PagedResponse<List<GetProductDto>>(data, validFilter.PageNumber, validFilter.PageSize, totalRecords);
    }

    public async Task<Response<GetProductDto>> CreateAsync(CreateProductDto request)
    {
        var product = mapper.Map<Product>(request);
        await context.Products.AddAsync(product);
        var result = await context.SaveChangesAsync();

        var data = mapper.Map<GetProductDto>(product);
        return result == 0
            ? new Response<GetProductDto>(HttpStatusCode.BadRequest, "Product not added!")
            : new Response<GetProductDto>(data);
    }

    public async Task<Response<GetProductDto>> GetAsync(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
            return new Response<GetProductDto>(HttpStatusCode.BadRequest, "Product not found");

        var data = mapper.Map<GetProductDto>(product);
        return new Response<GetProductDto>(data);
    }


    public async Task<Response<GetProductDto>> UpdateAsync(int id, UpdateProductDto request)
    {
        var existing = await context.Products.FindAsync(id);
        if (existing == null)
            return new Response<GetProductDto>(HttpStatusCode.BadRequest, "Product not found");

        existing.Name = request.Name;
        existing.Price = request.Price;

        var result = await context.SaveChangesAsync();
        var data = mapper.Map<GetProductDto>(existing);
        return result == 0
            ? new Response<GetProductDto>(HttpStatusCode.BadRequest, "Product not updated!")
            : new Response<GetProductDto>(data);
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
            return new Response<string>(HttpStatusCode.BadRequest, "Product not found");

        context.Products.Remove(product);
        var result = await context.SaveChangesAsync();

        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Product not deleted!")
            : new Response<string>("Product deleted!");
    }
}