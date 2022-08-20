using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Apply.com.Models
{
    public class User : IdentityUser
    {
        [MaxLength(20)]
        public string FirstName { get; set; }
        [MaxLength(60)]
        public string LastName { get; set; }
        [MaxLength(10)]
        public string? Gender { get; set; }
    }
}
