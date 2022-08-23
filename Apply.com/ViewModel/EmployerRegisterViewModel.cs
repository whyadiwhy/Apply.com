using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apply.com.ViewModel
{
    public class EmployerRegisterViewModel
    {
        [Required, MaxLength(20)]
        public string FirstName { get; set; }

        [Required, MaxLength(60)]
        public string LastName { get; set; }

        [Required, MaxLength(60)]
        [EmailAddress]
        public string Email { get; set; }

        [NotMapped]
        public IFormFile imageFile { get; set; }
        [Display(Name = "Employer Image")]
        public string imageURL { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
