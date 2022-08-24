using MicroServicesUser.Database;
using MicroServicesUser.Database.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MicroServicesUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeRegisterController : ControllerBase
    {
        DatabaseContext db;
        public EmployeeRegisterController()
        {
            db = new DatabaseContext();
        }
        [HttpGet]
        public IEnumerable<EmployeeRegister> Get()
        {
            return db.EmployeeRegisters.ToList();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public EmployeeRegister Get(int id)
        {
            return db.EmployeeRegisters.Find(id);
        }

        // POST api/<UserController>
        [HttpPost]
        public IActionResult Post([FromBody] EmployeeRegister model)
        {
            try
            {
                db.EmployeeRegisters.Add(model);
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
