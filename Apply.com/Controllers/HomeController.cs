using Apply.com.Data;
using Apply.com.Models;
using Apply.com.ViewModel.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Apply.com.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var jobs = _context.Jobs
                .Where(x => x.Filled == false)
                .ToList();
            var trendings = _context.Jobs
                .Where(b => b.CreatedAt.Month == DateTime.Now.Month)
                .Where(x => x.Filled == false)
                .ToList();
            var model = new TrendingJobViewModel
            {
                Trendings = trendings,
                Jobs = jobs
            };
            return View(model);
        }
        [Route("jobs/{id}/details")]
        public async Task<IActionResult> JobDetails(int id)
        {
            //ViewBag.message = "You can't do this action";
            var job = _context.Jobs.SingleOrDefault(x => x.Id == id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var applied = false;
            if (user != null)
                applied = _context.Applicants.Where(x => x.Job == job).Any(x => x.User == user);

            var model = new JobDetailsViewModel
            {
                Job = job,
                IsApplied = applied
            };
            return View(model);
        }
        [Route("search")]
        public IActionResult Search()
        {
            List<Job> jobs;

            string position = HttpContext.Request.Query["position"].ToString();
            string location = HttpContext.Request.Query["location"].ToString();
            if (position.Length > 0 && location.Length > 0)
            {
                jobs = _context.Jobs.Where(x => x.Title.Contains(position))
                    .ToList();
            }
            else if (location.Length == 0)
            {
                jobs = _context.Jobs.Where(x => x.Title.Contains(position))
                    .ToList();
            }
            else
            {
                jobs = _context.Jobs.Where(x => x.Location.Contains(location))
                    .ToList();
            }


            return View(jobs);
        }
    }
}