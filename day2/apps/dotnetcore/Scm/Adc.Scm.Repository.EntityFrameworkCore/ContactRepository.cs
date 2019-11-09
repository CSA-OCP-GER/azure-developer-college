using Adc.Scm.DomainObjects;
using Adc.Scm.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adc.Scm.Repository.EntityFrameworkCore
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactDbContext _context;

        public ContactRepository(ContactDbContext context)
        {
            _context = context;
        }

        public async Task<Contact> Add(Contact contact)
        {
            await _context.Database.EnsureCreatedAsync();

            contact.Id = Guid.NewGuid();
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task<Contact> Delete(Guid id)
        {
            await _context.Database.EnsureCreatedAsync();

            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);
            if (null == contact)
                return null;

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task<Contact> Get(Guid id)
        {
            await _context.Database.EnsureCreatedAsync();

            return await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Contact>> Get()
        {
            await _context.Database.EnsureCreatedAsync();

            return await _context.Contacts.ToListAsync();
        }

        public async Task<Contact> Save(Contact contact)
        {
            await _context.Database.EnsureCreatedAsync();

            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
            return contact;
        }
    }
}
