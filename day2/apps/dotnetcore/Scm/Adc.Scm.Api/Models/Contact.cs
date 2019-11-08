using System;
using System.ComponentModel.DataAnnotations;

namespace Adc.Scm.Api.Models
{
    public class Contact
    {
        public Guid? Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Company { get; set; }
    }
}
