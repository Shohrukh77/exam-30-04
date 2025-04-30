using AutoMapper;
using Domain.DTOs.Customer;
using Domain.DTOs.Order;
using Domain.DTOs.OrderItem;
using Domain.DTOs.Product;
using Domain.Entities;

namespace Infrastructure.AuMapper;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {
        CreateMap<Customer, CreateCustomerDto>();
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<Product, CreateProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<Order, CreateOrderDto>();
        CreateMap<CreateOrderDto, Order>();
        CreateMap<OrderItem, CreateOrderItemDto>();
        CreateMap<CreateOrderItemDto, OrderItem>();
    }
}