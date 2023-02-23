using ContactList.Entities;
using ContactList.Models;
using ContactList.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactList.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IContactService _contactService;
        private readonly ContactListDbContext _dbContext;
        private readonly bool _isUserLoggedIn; 


        public ContactController(ILogger<ContactController> logger, ContactListDbContext dbContext, IContactService contactService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contactService = contactService;
            var session = httpContextAccessor.HttpContext.Session;
            _isUserLoggedIn = session.Get("IsUserLoggedIn") is byte[] value && value.Length > 0 && BitConverter.ToBoolean(value, 0);
        }

        //Metoda ta pobiera listę kontaktów z bazy danych i przekazuje ją do widoku. 
        public IActionResult Index()
        {
            var contacts = _dbContext.Contacts.ToList();
            return View(contacts);
        }

        [HttpPost]
        public IActionResult Register(Contact contact)
        {
            // Walidacja danych wejściowych
            if (!ModelState.IsValid)
            {
                return View("Index", _dbContext.Contacts.ToList());
            }

            // Sprawdzenie, czy konto o podanym adresie email już istnieje
            var existingContact = _dbContext.Contacts.FirstOrDefault(c => c.Email == contact.Email);
            if (existingContact != null)
            {
                ModelState.AddModelError("Email", "Konto o podanym adresie email już istnieje");
                return View("Index", _dbContext.Contacts.ToList());
            }

            // Dodanie nowego kontaktu do bazy danych
            _dbContext.Contacts.Add(contact);
            _dbContext.SaveChanges();

            // Przekierowanie na stronę logowania
            return View("Index", _dbContext.Contacts.ToList());
        }


        //jest tu problem, ponieważ po rejestracji trzeba zresetowć aplikacje, żeby się zalogować
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Sprawdzenie, czy konto o podanym adresie email i haśle istnieje
            var contact = _dbContext.Contacts.FirstOrDefault(c => c.Email == email && c.Password == password);
            if (contact == null)
            {
                ModelState.AddModelError("", "Niepoprawny adres email lub hasło");
                return View("Index", _dbContext.Contacts.ToList());
            }

            // Zalogowanie użytkownika
            HttpContext.Session.Set("IsUserLoggedIn", BitConverter.GetBytes(true));


            // Przekierowanie na stronę główną
            return View("Index", _dbContext.Contacts.ToList());
        }

        [HttpPost]
        public IActionResult Add(Contact contact, string otherSubcategory)
        {
            if (_isUserLoggedIn == true)
            {
                 contact.Subcategory = contact.Subcategory == "inne" ? otherSubcategory : contact.Subcategory;
                _dbContext.Add(contact);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
           else
           {
             ModelState.AddModelError("", "Nie jesteś zalogowany, nie możesz dodawać kontaktu");
             return RedirectToAction(nameof(Index));
           }
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = _dbContext.Contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }
        //w tym przypadku używamy POST, ponieważ formularz HTML nie obsługuje bezpośrednio metody DELETE
        [HttpPost]
        public IActionResult Delete(string? emailToDelete)
        {
            if (_isUserLoggedIn == true)
            {
                if (emailToDelete == null)
                {
                    return NotFound();
                }

                var contact = _dbContext.Contacts.FirstOrDefault(c => c.Email == emailToDelete);
                if (contact == null)
                {
                    return NotFound();
                }
                _dbContext.Remove(contact);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Nie jesteś zalogowany, nie możesz usuwać kontaktów");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public IActionResult Update(Contact contact, string emailToUpdate)
        {
            if (_isUserLoggedIn == true)
            {
                _contactService.Update(contact, emailToUpdate);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Nie jesteś zalogowany, nie możesz aktualizować kontaktów");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
