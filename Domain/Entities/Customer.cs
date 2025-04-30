namespace Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    
    public List<Order> Orders { get; set; } 
}