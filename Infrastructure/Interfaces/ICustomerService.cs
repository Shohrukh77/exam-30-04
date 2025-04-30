using Domain.DTOs.Customer;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface ICustomerService
{
    Task<Response<List<GetCustomerDto>>> GetAllAsync(CustomerFilter  filter);
    Task<Response<GetCustomerDto>> CreateAsync(CreateCustomerDto request);
    Task<Response<GetCustomerDto>> GetAsync(int id);
    Task<Response<GetCustomerDto>> UpdateAsync(int id, UpdateCustomerDto request);
    Task<Response<string>> DeleteAsync(int id);
}