

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task6MVC.Data;
using Task6MVC.Models;

namespace Task6MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToList();

            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            ModelState.Remove(nameof(Product.Category));
            ModelState.Remove(nameof(Product.Supplier));

            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            LoadDropdowns();
            return View(product);
        }

        private void LoadDropdowns()
        {
            ViewBag.Categories = new SelectList(
                _context.Categories.ToList(),
                "Id",
                "Name"
            );

            ViewBag.Suppliers = new SelectList(
                _context.Suppliers.ToList(),
                "Id",
                "Name"
            );
        }
    }
}
