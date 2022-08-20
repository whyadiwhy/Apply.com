using Microsoft.AspNetCore.Identity;

namespace Apply.com.Models
{
    public class Applicant
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Job Job { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    
    

}
