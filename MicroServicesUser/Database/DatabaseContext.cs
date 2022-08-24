using MicroServicesRegisterLogin.Database.Entities;
using MicroServicesUser.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroServicesUser.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<EmployeeRegister> EmployeeRegisters { get; set; }
        public DbSet<EmployerRegister> EmployerRegisters { get; set; }
        public DbSet<Login> Logins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DEL1-LHP-N82115;Database=LoginRegisterMicroservices;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
