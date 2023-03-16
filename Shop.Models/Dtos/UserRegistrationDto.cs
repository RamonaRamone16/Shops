using Shop.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shop.Models.Dtos
{
    public class UserRegistrationDto
    {
        public string FirstName { get; set; }       
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public Roles Role { get; set; }

    }
}
