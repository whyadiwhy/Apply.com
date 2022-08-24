using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MicroServicesRegisterLogin.Database.Entities
{
    public class Login
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
