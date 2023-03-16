
namespace Shop.Models.Entities
{
    public class Product : BaseEntity<string>
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Shop Shop { get; set; }
    }
}
