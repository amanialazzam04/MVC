
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task6MVC.Data;
using Task6MVC.Models;

namespace Task6MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.Employee)
                .ToList();

            return View(orders);
        }



        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Employees = new SelectList(
                _context.Employees.ToList(),
                "Id",
                "FullName"
            );

            return View();
        }



        [HttpPost]
        public IActionResult Create(Order order)
        {
            ModelState.Remove("Employee");

            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            foreach (var item in ModelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    Console.WriteLine(item.Key + ": " + error.ErrorMessage);
                }
            }

            ViewBag.Employees = new SelectList(
                _context.Employees.ToList(),
                "Id",
                "FullName"
            );

            return View(order);
        }








        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            ViewBag.Employees = new SelectList(
                _context.Employees.ToList(),
                "Id",
                "FullName",
                order.EmployeeId
            );

            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(Order order)
        {
            ModelState.Remove("Employee");

            if (ModelState.IsValid)
            {
                _context.Orders.Update(order);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Employees = new SelectList(
                _context.Employees.ToList(),
                "Id",
                "FullName",
                order.EmployeeId
            );

            return View(order);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.Employee)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }




    }
}
