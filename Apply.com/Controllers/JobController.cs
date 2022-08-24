using Apply.com.Data;
using Apply.com.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace Apply.com.Controllers
{
    public class JobController : Controller
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private UserManager<User> _userManager; //to manage users e.g. registering new users, validating credentials and loading user information. 
        public IWebHostEnvironment _hostEnvironment;

        public JobController(ApplicationDbContext context, UserManager<User> userManager, ILogger<JobController> logger, IWebHostEnvironment hostEnvironment) {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }
        [Route("jobs")]
        public IActionResult Index()
        {
            var jobs = _context.Jobs.ToList();

            return View(jobs);
        }
        [Route("jobs/create")]
        [Authorize(Roles = "Employer")]
        public IActionResult Create()
        {
            return View();
        }
        [Route("jobs/save")]
        [Authorize(Roles = "Employer")]
        [HttpPost]
        public async Task<IActionResult> Save(Job model,IFormFile image)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath; //gets the location of wwwroot folder
            if (ModelState.IsValid)
            {
                TempData["type"] = "success";
                TempData["message"] = "Job posted successfully";

                _logger.LogInformation("Job/save: {@model}",model);
                var user = await _userManager.GetUserAsync(HttpContext.User);
                model.User = user;
                

                if (image != null) {
                    string fileName = image.FileName;
                    var uploads = Path.Combine(wwwRootPath, @"img/EmployerLogo");
                    var extension = Path.GetExtension(image.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create)) {
                        image.CopyTo(fileStreams);
                    }
                    user.imageURL = fileName;

                }
                model.companyImgURL = user.imageURL;
                _context.Jobs.Add(model);
                await _context.SaveChangesAsync();

                for (int i = 0; i < _context.Jobs.Count();i++) {

                    _logger.LogInformation("image URL in while create=ing JOB: " + _context.Jobs.ToList()[i].companyImgURL); }

                return RedirectToActionPermanent("Index", "Home");
            }
            var user2 = await _userManager.GetUserAsync(HttpContext.User);
           
            
            
            return View("Create", model);
        }
        [HttpPost]
        public async Task<IActionResult> Apply(int id)
        {
            var job = _context.Jobs.SingleOrDefault(x => x.Id == id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return RedirectToActionPermanent("Login", "Account");
            }
            else
            {
                if (!User.IsInRole("Employee"))
                {
                    TempData["message"] = "You can't do this action";
                    return RedirectToActionPermanent("JobDetails", "Home", new { id });
                }
            }

            var apply = new Applicant
            {
                User = user,
                Job = job,
                CreatedAt = DateTime.Now
            };
            _context.Applicants.Add(apply);
            MailMessage mail = new MailMessage();
            mail.To.Add(user.Email);
            mail.From = new MailAddress("aniket.sharma@globallogic.com");
            mail.Body = "Hello friends, testing karlo";
            mail.Subject = "This is a testing mail";
            //An SMTP (Simple Mail Transfer Protocol) server is an application that's primary
            //purpose is to send,receive, and/or relay outgoing mail between email senders and receivers.
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("aniket.sharma@globallogic.com", "P@ssword@68"); // Enter seders User name and password       
            smtp.EnableSsl = true;
            smtp.Send(mail);

            await _context.SaveChangesAsync();

            return RedirectToActionPermanent("JobDetails", "Home", new { id });
        }
        [Route("mark-as-filled/{id}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> MarkAsFilled(int id)
        {
            var job = _context.Jobs.SingleOrDefault(x => x.Id == id);
            job.Filled = true;
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();

            return RedirectToActionPermanent("Index", "Dashboard");
        }
        [HttpPost]
        [Authorize(Roles = "Employer")]
        [Route("employer/jobs/{id}/destroy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Destroy(int id)
        {
            var job = _context.Jobs.SingleOrDefault(x => x.Id == id);
            var jobA = _context.Applicants.SingleOrDefault(x => x.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            _context.Applicants.Remove(jobA);
            await _context.SaveChangesAsync();

            TempData["type"] = "success";
            TempData["message"] = "Job deleted successfully";

            return RedirectToActionPermanent("Index", "Dashboard");
        }
    }
}
