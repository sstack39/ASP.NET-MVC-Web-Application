using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackWebApp.Data;
using StackWebApp.Models;
using System.Collections.Generic;


public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }
    // GET: Product
    public async Task<IActionResult> Index()
    {
        return View(await _context.Products.ToListAsync());
    }
    //public IActionResult Index()
    //{
    //    var products = GetSampleProducts(); // Replace with your data source
    //    return View(products);
        
    //}

    private List<ProductModel> GetSampleProducts()
    {
        // Sample data - replace with your actual data retrieval logic
        return new List<ProductModel>
        {
            new ProductModel { SerialNumber = "001", Name = "Product 1", Category = "Category A", Price = 19.99m },
            new ProductModel { SerialNumber = "002", Name = "Product 2", Category = "Category B", Price = 29.99m },
            // Add more products...
        };
    }

    // GET: Product/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Id,SerialNumber,Name,Category,Price")] ProductModel productModel)
    {
        if (ModelState.IsValid)
        {
            // Add logic to save the product to your database
            _context.Products.Add(productModel);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(productModel);
    }
}