using System.Net;
using AutoMapper;
using Domain.DTOs.Customer;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class CustomerService(ApplicationDbContext context, IMapper mapper) : ICustomerService
{
    public async Task<Response<List<GetCustomerDto>>> GetAllAsync(CustomerFilter filter)
    {
        var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);
        var query = context.Customers.AsQueryable();

        if (filter.FullName != null)
            query = query.Where(x => x.FullName.ToLower().Contains(filter.FullName.ToLower()));
        
        if (filter.Email != null)
            query = query.Where(x => x.Email.Contains(filter.Email));
        
        if (filter.PhoneNumber != null)
            query = query.Where(x => x.PhoneNumber.Contains(filter.PhoneNumber));


        var mapped = mapper.Map<List<GetCustomerDto>>(query.ToList());
        var totalRecords = mapped.Count;

        var data = mapped
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        return new PagedResponse<List<GetCustomerDto>>(data, validFilter.PageNumber, validFilter.PageSize, totalRecords);
    }

    public async Task<Response<GetCustomerDto>> CreateAsync(CreateCustomerDto request)
    {
        var customer = mapper.Map<Customer>(request);
        await context.Customers.AddAsync(customer);
        var result = await context.SaveChangesAsync();

        var data = mapper.Map<GetCustomerDto>(customer);
        return result == 0
            ? new Response<GetCustomerDto>(HttpStatusCode.BadRequest, "Customer not added!")
            : new Response<GetCustomerDto>(data);
    }

    public async Task<Response<GetCustomerDto>> GetAsync(int id)
    {
        var customer = await context.Customers.FindAsync(id);
        if (customer == null)
            return new Response<GetCustomerDto>(HttpStatusCode.BadRequest, "Customer not found");

        var data = mapper.Map<GetCustomerDto>(customer);
        return new Response<GetCustomerDto>(data);
    }


    public async Task<Response<GetCustomerDto>> UpdateAsync(int id, UpdateCustomerDto request)
    {
        var existing = await context.Customers.FindAsync(id);
        if (existing == null)
            return new Response<GetCustomerDto>(HttpStatusCode.BadRequest, "Customer not found");

        existing.Email = request.Email;
        existing.FullName = request.FullName;
        existing.PhoneNumber = request.PhoneNumber;

        var result = await context.SaveChangesAsync();
        var data = mapper.Map<GetCustomerDto>(existing);
        return result == 0
            ? new Response<GetCustomerDto>(HttpStatusCode.BadRequest, "Customer not updated!")
            : new Response<GetCustomerDto>(data);
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var customer = await context.Customers.FindAsync(id);
        if (customer == null)
            return new Response<string>(HttpStatusCode.BadRequest, "Customer not found");

        context.Customers.Remove(customer);
        var result = await context.SaveChangesAsync();

        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Customer not deleted!")
            : new Response<string>("Customer deleted!");
    }
}