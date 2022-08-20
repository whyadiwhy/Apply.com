using System.ComponentModel.DataAnnotations;

namespace Apply.com.ViewModel
{
    public class EmployeeRegisterViewModel
    {
        [Required, MaxLength(20), Display(Name = "First Name", Prompt = "First Name")]
        public string FirstName { get; set; }

        [Required, MaxLength(60), Display(Name = "Last Name", Prompt = "Last Name")]
        public string LastName { get; set; }

        [Required, MaxLength(60), Display(Name = "Email", Prompt = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Password", Prompt = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password)), Display(Name = "Confirm Password", Prompt = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
