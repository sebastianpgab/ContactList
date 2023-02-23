using ContactList.Entities;
using ContactList.Models;
using ContactList.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace ContactList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ContactListDbContext _dbContext;


        public HomeController(ILogger<HomeController> logger, ContactListDbContext contactListDb)
        {
            _logger = logger;
            _dbContext = contactListDb;
        }

        public IActionResult Index()
        {
            var contacts = _dbContext.Contacts.ToList();
            return View(contacts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}