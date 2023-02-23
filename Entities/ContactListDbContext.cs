using ContactList.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactList.Entities
{
    public class ContactListDbContext : DbContext
    {
        private string _connectionString = "Server=SEBASTIANPGAB\\SQLEXPRESS; Database=ContactList; Trusted_Connection=True";
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

    }
}
