using Adc.Scm.DomainObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adc.Scm.Repository.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> Add(Contact contact);
        Task<Contact> Save(Contact contact);
        Task<Contact> Delete(Guid id);
        Task<Contact> Get(Guid id);
        Task<List<Contact>> Get();
    }
}
