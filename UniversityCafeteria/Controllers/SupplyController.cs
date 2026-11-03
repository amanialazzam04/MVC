using Microsoft.AspNetCore.Mvc;

namespace UniversityCafeteria.Controllers
{
    public class SupplyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Products()
        {
            ViewData["Title"] = "Cafeteria Products";
            ViewData["ManagerName"] = "Ahmad Ali";
            ViewData["ProductCount"] = 5;

            string[] products =
            {
                "Water",
                "Orange Juice",
                "Coffee",
                "Sandwich",
                "Chocolate"
            };

            ViewData["Products"] = products;

            return View();
        }

        public IActionResult Suppliers()
        {
            ViewBag.Title = "Our Suppliers";
            ViewBag.SupplierCount = 3;

            string[] suppliers =
            {
                "Fresh Food Company",
                "Happy Drinks Company",
                "Daily Supplies Company"
            };

            ViewBag.Suppliers = suppliers;

            return View();
        }

        [HttpGet]
        public IActionResult CreateRequest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitRequest()
        {
            string employeeName = Request.Form["employeeName"];
            string productName = Request.Form["productName"];
            string quantity = Request.Form["quantity"];
            string requiredDate = Request.Form["requiredDate"];

            if (string.IsNullOrEmpty(employeeName))
            {
                return Content("Employee Name is required!");
            }

            TempData["Message"] =
                $"Supply request submitted successfully! Employee: {employeeName}, Product: {productName}, Quantity: {quantity}, Date: {requiredDate}";

            return RedirectToAction("About");
        }

        public IActionResult About()
        {
            return View();
        }

        public string CheckQuantity(int quantity)
        {
            if (quantity < 20)
            {
                return "Small Order";
            }
            else if (quantity <= 50)
            {
                return "Medium Order";
            }
            else
            {
                return "Large Order";
            }
        }

        public IActionResult CheckOrder()
        {
            int quantity = 60;

            string result = CheckQuantity(quantity);

            return Content(result);
        }
    }
}