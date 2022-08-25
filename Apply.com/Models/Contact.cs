using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Apply.com.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Required, MaxLength(40), Display(Name = "First Name", Prompt = "First Name")]
        public string FirstName { get; set; }
        [Required, MaxLength(40), Display(Name = "Last Name", Prompt = "Last Name")]
        public string LastName { get; set; }
        [Required, MaxLength(60), Display(Name = "Email", Prompt = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(500), Display(Name = "Comment")]
        public string Comment { get; set; }
    }
}
