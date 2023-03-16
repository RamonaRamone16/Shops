using Microsoft.AspNetCore.Identity;

namespace Shop.Models.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LasrName { get; set; }

        public List<Shop> Shops { get; set; }
    }
}
