using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> Products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 999.99m },
        new Product { Id = 2, Name = "Mouse", Price = 29.99m },
        new Product { Id = 3, Name = "Keyboard", Price = 79.99m }
    };

    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get()
    {
        return Ok(Products);
    }

    [HttpGet("{id}")]
    public ActionResult<Product> Get(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();
        
        return Ok(product);
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}