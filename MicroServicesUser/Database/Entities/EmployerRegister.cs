using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MicroServicesRegisterLogin.Database.Entities
{
    public class EmployerRegister
    {
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string FirstName { get; set; }

        [Required, MaxLength(60)]
        public string LastName { get; set; }

        [Required, MaxLength(60)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
