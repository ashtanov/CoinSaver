using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CoinSaver.Models;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CoinSaver.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BackendController : Controller
    {
        private readonly RoleManager<CSRole> _roleManager;
        private readonly SignInManager<CSUser> _signInManager;
        private readonly UserManager<CSUser> _userManager;
        private readonly ILogger _logger;
        public BackendController(UserManager<CSUser> userManager,
            SignInManager<CSUser> signInManager,
            RoleManager<CSRole> roleManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger<BackendController>();
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
    }
}
