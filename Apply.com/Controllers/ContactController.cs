using Apply.com.Models;
using Microsoft.AspNetCore.Mvc;

namespace Apply.com.Controllers
{
    public class ContactController : Controller
    {
        //private readonly IConfiguration configuration;
        string serviceUrl;

        public ContactController(IConfiguration configuration)
        {
            serviceUrl = configuration["ServiceUrl"];
        }
        public IActionResult Index()
        {
            IEnumerable<Contact> facilities = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(serviceUrl);

                var responseTask = client.GetAsync(serviceUrl);
                responseTask.Wait();

                var contacts = responseTask.Result;
                if (contacts.IsSuccessStatusCode)
                {
                    var readTask = contacts.Content.ReadFromJsonAsync<IList<Contact>>();
                    readTask.Wait();

                    facilities = readTask.Result;
                }
                else
                {
                    facilities = Enumerable.Empty<Contact>();

                    ModelState.AddModelError(String.Empty, "Server Error, Plaease contact administrator.");
                }
            }
            Console.WriteLine(facilities);
            return View(facilities);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Contact facilities)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2565/api/contact");

                var postTask = client.PostAsJsonAsync<Contact>("Contact", facilities);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError(String.Empty, "Server Error. Please contact administrator");
            return View(facilities);
        }
    }
}
