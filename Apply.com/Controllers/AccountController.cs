using Apply.com.Data;
using Apply.com.Models;
using Apply.com.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Apply.com.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        public IWebHostEnvironment _hostEnvironment;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signManager, RoleManager<IdentityRole> roleManager, 
            ApplicationDbContext context, ILogger<AccountController> logger,
            IWebHostEnvironment hostEnvironment) {
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }
        [HttpGet]
        [Route("employer/register")]
        public IActionResult EmployerRegister()
        {
            return View();
        }
        [HttpPost]
        [Route("employer/register")]
        public async Task<IActionResult> EmployerRegister(
            [Bind("FirstName", "LastName", "Email", "Password", "ConfirmPassword")]
            EmployerRegisterViewModel model, IFormFile image)
        {
            if (ModelState.IsValid)
            {

                var user = new User
                {
                    UserName = model.FirstName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email  
                };

                string wwwRootPath = _hostEnvironment.WebRootPath; //gets the location of wwwroot folder
                if (image != null) {
                    string fileName = image.FileName;
                    var uploads = Path.Combine(wwwRootPath, @"img/EmployerProfile");
                    var extension = Path.GetExtension(image.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create)) {
                        image.CopyTo(fileStreams);
                    }
                    user.imageURL = fileName;

                }
                _logger.LogInformation(model.imageURL);
                var result = await _userManager.CreateAsync(user, model.Password);
                //IdentityResult roleResult = await _roleManager.CreateAsync(new IdentityRole("Employee"));

                if (result.Succeeded)
                {
                    bool checkRole = await _roleManager.RoleExistsAsync("Employer");
                    if (!checkRole)
                    {
                        var role = new IdentityRole();
                        role.Name = "Employer";
                        await _roleManager.CreateAsync(role);

                        await _userManager.AddToRoleAsync(user, "Employer");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Employer");
                    }

                   
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View();
        }
        [HttpGet]
        [Route("employee/register")]
        public IActionResult EmployeeRegister()
        {
            return View();
        }
        [HttpPost]
        [Route("employee/register")]
        public async Task<IActionResult> EmployeeRegister(
            [Bind("FirstName", "LastName", "Email", "Password", "ConfirmPassword")]
            EmployeeRegisterViewModel model,IFormFile resume)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.FirstName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    imageURL=model.ResumeURL
                };
                string wwwRootPath = _hostEnvironment.WebRootPath; //gets the location of wwwroot folder
                if (resume != null) {
                    string fileName = resume.FileName;
                    var uploads = Path.Combine(wwwRootPath, @"resume");
                    var extension = Path.GetExtension(resume.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create)) {
                        resume.CopyTo(fileStreams);
                    }
                    user.imageURL = fileName;

                }
                _logger.LogInformation(model.ResumeURL);
                var result = await _userManager.CreateAsync(user, model.Password);
                //IdentityResult roleResult = await _roleManager.CreateAsync(new IdentityRole("Employee"));

                if (result.Succeeded)
                {
                    bool checkRole = await _roleManager.RoleExistsAsync("Employee");
                    if (!checkRole)
                    {
                        var role = new IdentityRole();
                        role.Name = "Employee";
                        await _roleManager.CreateAsync(role);

                        await _userManager.AddToRoleAsync(user, "Employee");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Employee");
                    }

                    //await _signManager.SignInAsync(user, false);
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }
        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = "")
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

                var userName = user.UserName;

                var result = await _signManager.PasswordSignInAsync(userName,
                    model.Password, model.RememberMe, lockoutOnFailure: false);


                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        [Route("employee/edit-profile")]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return View(user);
        }
        [Authorize(Roles = "Employee")]
        [HttpPost]
        [Route("employee/update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] User model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Gender = model.Gender;

            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return RedirectToActionPermanent("EditProfile", "Account");
        }
    }
}
