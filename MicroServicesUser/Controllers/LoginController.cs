using MicroServicesRegisterLogin.Database.Entities;
using MicroServicesUser.Database;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MicroServicesRegisterLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        DatabaseContext db;
        public LoginController()
        {
            db = new DatabaseContext();
        }
        [HttpGet]
        public IEnumerable<Login> Get()
        {
            return db.Logins.ToList();
        }

        // GET api/<LoginController>/5
        [HttpGet("{id}")]
        public Login Get(int id)
        {
            return db.Logins.Find(id);
        }

        // POST api/<LoginController>
        [HttpPost]
        public IActionResult Post([FromBody] Login model)
        {
            try
            {
                db.Logins.Add(model);
                db.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
