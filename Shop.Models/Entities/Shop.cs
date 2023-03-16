
namespace Shop.Models.Entities
{
    public class Shop : BaseEntity<string>
    {
        public string Name { get; set; }
        public User Manager { get; set; }

        public List<Product> Products { get; set; }
    }
}
