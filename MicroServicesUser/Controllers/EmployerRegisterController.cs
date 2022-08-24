using MicroServicesRegisterLogin.Database.Entities;
using MicroServicesUser.Database;
using MicroServicesUser.Database.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MicroServicesRegisterLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerRegisterController : ControllerBase
    {
        DatabaseContext db;
        public EmployerRegisterController()
        {
            db = new DatabaseContext();
        }
        [HttpGet]
        public IEnumerable<EmployerRegister> Get()
        {
            return db.EmployerRegisters.ToList();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public EmployerRegister Get(int id)
        {
            return db.EmployerRegisters.Find(id);
        }

        // POST api/<UserController>
        [HttpPost]
        public IActionResult Post([FromBody] EmployerRegister model)
        {
            try
            {
                db.EmployerRegisters.Add(model);
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
