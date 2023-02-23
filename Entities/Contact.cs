using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactList.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ContactCategory Category { get; set; }
        public string Subcategory { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public enum ContactCategory
    {
        Business,
        Personal,
        Other
    }
}
