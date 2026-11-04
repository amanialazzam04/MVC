using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using UniversityCafeteria.Models;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;



namespace UniversityCafeteria.Controllers
{
    public class SupplyRequestController : Controller
    {
        private static List<SupplyRequest> confirmedRequests = new List<SupplyRequest>();

        private static int nextRequestId = 1;
        public IActionResult Index()
        {
            ViewBag.PreferredPriority = Request.Cookies["PreferredPriority"];

            return View();
        }

        [HttpPost]
        public IActionResult AddItem(
            string productName,
            string category,
            int quantity,
            decimal unitPrice,
            string priority)
        {
            // Validate Product Name
            if (string.IsNullOrEmpty(productName))
            {
                return Content("Product Name is required!");
            }

            // Validate Quantity and Unit Price
            if (quantity <= 0 || unitPrice <= 0)
            {
                return Content("Quantity and Unit Price must be greater than zero!");
            }

            // Get existing products from Session
            List<SupplyItem> items = GetDraftItems();

            // Create a new product
            SupplyItem item = new SupplyItem
            {
                ProductName = productName,
                Category = category,
                Quantity = quantity,
                UnitPrice = unitPrice,
                Priority = priority
            };

            // Add product to the list
            items.Add(item);

            // Save the list in Session
            string json = JsonSerializer.Serialize(items);

            HttpContext.Session.SetString("DraftItems", json);

            return Content("Product added successfully! Total products: " + items.Count);
        }
        public IActionResult Review()
        {
            List<SupplyItem> items = GetDraftItems();

            return View(items);
        }
        [HttpPost]
        public IActionResult ConfirmRequest()
        {
            List<SupplyItem> items = GetDraftItems();

            // Check if the request is empty
            if (items.Count == 0)
            {
                return Content("Cannot confirm an empty request!");
            }

            // Calculate Subtotal
            decimal subtotal = items.Sum(item => item.TotalPrice);

            // Determine highest priority
            string priority = "Normal";

            if (items.Any(item => item.Priority == "Emergency"))
            {
                priority = "Emergency";
            }
            else if (items.Any(item => item.Priority == "Urgent"))
            {
                priority = "Urgent";
            }

            // Calculate Preparation Fee
            decimal preparationFee = 2;

            if (priority == "Urgent")
            {
                preparationFee = 5;
            }
            else if (priority == "Emergency")
            {
                preparationFee = 10;
            }

            // Calculate Discount
            decimal discount = 0;

            if (subtotal > 200)
            {
                discount = subtotal * 0.15m;
            }
            else if (subtotal > 100)
            {
                discount = subtotal * 0.10m;
            }

            // Create confirmed request
            SupplyRequest request = new SupplyRequest
            {
                RequestId = nextRequestId++,
                Items = items,
                Subtotal = subtotal,
                PreparationFee = preparationFee,
                Discount = discount,
                FinalTotal = subtotal + preparationFee - discount,
                Status = "Confirmed",
                RequestDate = DateTime.Now
            };

            // Save request in memory
            confirmedRequests.Add(request);

            // Clear Session draft
            HttpContext.Session.Remove("DraftItems");

            return Content("Request confirmed successfully! Request ID: " + request.RequestId);
        }
        // Get products from Session
        public IActionResult MyRequests()
        {
            return View(confirmedRequests);
        }
        public IActionResult ClearDraft()
        {
            HttpContext.Session.Remove("DraftItems");

            return RedirectToAction("Index");
        }
        private List<SupplyItem> GetDraftItems()
        {
            string json = HttpContext.Session.GetString("DraftItems");

            if (string.IsNullOrEmpty(json))
            {
                return new List<SupplyItem>();
            }

            return JsonSerializer.Deserialize<List<SupplyItem>>(json)
                   ?? new List<SupplyItem>();
        }
        [HttpPost]
        public IActionResult SavePreference(string priority)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7)
            };

            Response.Cookies.Append("PreferredPriority", priority, options);

            return RedirectToAction("Index");
        }
        public IActionResult About()
        {
            return View();
        }
    }
}