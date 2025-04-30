using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcApp.Models;
using Order = Domain.Entities.Order;

namespace MvcApp.Controllers;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Order order)
    {
        if (ModelState.IsValid)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(order);
    }

    public IActionResult Index()
    {
        var orders = _context.Orders.Include(o => o.Items).Include(o => o.Customer).ToList();
        return View(orders);
    }

    public IActionResult Details(int id)
    {
        var order = _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Include(o => o.Customer)
            .FirstOrDefault(o => o.Id == id);

        if (order == null) return NotFound();

        return View(order);
    }
}