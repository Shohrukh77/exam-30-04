using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models;

public class Customer
{
    public int Id { get; set; }

    [Required(ErrorMessage = "ФИО обязательно.")]
    [StringLength(100, ErrorMessage = "ФИО не может быть длиннее 100 символов.")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Номер телефона обязателен.")]
    [Phone(ErrorMessage = "Неверный формат номера телефона.")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "Email обязателен.")]
    [EmailAddress(ErrorMessage = "Неверный формат email.")]
    public string Email { get; set; } = null!;
}