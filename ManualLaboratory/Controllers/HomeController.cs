using ManualLaboratory.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ManualLaboratory.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public HomeController(ILogger<HomeController> logger,UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> CreateRoles()
        {
            var roles = new[] { "Admin", "Recep" };
            foreach (var role in roles)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role);
                if (!roleExist)
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }
            return View("index", "User added successflly");
        }
        public async Task<IActionResult> AddRoleToUsers()
        {
            var Mohammed = await _userManager.FindByNameAsync("Mohammed@edu.sa");
            await _userManager.AddToRoleAsync(Mohammed, "Admin");
            var Ali = await _userManager.FindByNameAsync("Ali@edu.sa");
            await _userManager.AddToRoleAsync(Ali, "Recep");
            return View("index", "User added successflly");
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}