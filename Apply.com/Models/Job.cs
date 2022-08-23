using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apply.com.Models
{
    
    public class Job
    {
        public int Id { get; set; }

        [Required, MaxLength(255), Display(Name = "Job Title", Prompt = "Job Title")]
        public string Title { get; set; }

        [Required, Display(Name = "Job Description", Prompt = "Job Description")]
        public string Description { get; set; }

        [Required, Display(Name = "Location", Prompt = "Location")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Type is required"), Display(Name = "Type", Prompt = "Type")]
        public string Type { get; set; }

        [Display(Name = "Category", Prompt = "Category")]
        public string Category { get; set; }
        [Required, Display(Name = "Last Date", Prompt = "Last Date")]
        public DateTime LastDate { get; set; }

        [Required, Display(Name = "Company Name", Prompt = "Company Name")]
        public string CompanyName { get; set; }

        [Required, Display(Name = "Company Description", Prompt = "Company Description")]
        public string CompanyDescription { get; set; }

        [Display(Name = "Website", Prompt = "Website")]
        [Url]
        public string Website { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool Filled { get; set; } = false;

        public User User { get; set; }

        [NotMapped]
        public string? companyImgURL { get; set; }

        public List<Applicant> Applicants { get; set; }
    }
}
