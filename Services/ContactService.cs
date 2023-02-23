using ContactList.Entities;
using ContactList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactList.Services
{
    public interface IContactService
    {
        public void Update(Contact contact, string emailToUpdate);

    }
    //przeniesienie część logiki biznesowej do Serwisu, tak aby trochę oczyścić Kontroler
    public class ContactService : IContactService
    {
        private readonly ContactListDbContext _dbContext;

        public ContactService(ContactListDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Contact contact, string emailToUpdate)
        {
            var contactToUpdate = _dbContext.Contacts.FirstOrDefault(c => c.Email == emailToUpdate);
            if (contactToUpdate != null)
            {
                contactToUpdate.FirstName = contact.FirstName != null ? contact.FirstName : contactToUpdate.FirstName;
                contactToUpdate.LastName = contact.LastName != null ? contact.LastName : contactToUpdate.LastName;
                contactToUpdate.Email = contact.Email != null ? contact.Email : contactToUpdate.Email;
                contactToUpdate.Password = contact.Password != null ? contact.Password : contactToUpdate.Password;
                contactToUpdate.Category = contact.Category != null ? contact.Category : contactToUpdate.Category;
                contactToUpdate.Subcategory = contact.Subcategory != null ? contact.Subcategory : contactToUpdate.Subcategory;
                contactToUpdate.Phone = contact.Phone != null ? contact.Phone : contactToUpdate.Phone;
                contactToUpdate.DateOfBirth = contact.DateOfBirth != null ? contact.DateOfBirth : contactToUpdate.DateOfBirth;

                _dbContext.SaveChanges();
            }
        }





    }
}
