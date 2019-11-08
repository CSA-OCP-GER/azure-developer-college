using System;

namespace Adc.Scm.DomainObjects
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
    }
}
