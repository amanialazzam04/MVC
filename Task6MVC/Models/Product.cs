
namespace Task6MVC.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int CategoryId { get; set; }

        public int SupplierId { get; set; }

        public Category Category { get; set; } = null!;

        public Supplier Supplier { get; set; } = null!;
    }
}

