using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ENAEJJMLRSFG.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Addresses = new List<Address>();
        }

        public int Id { get; set; }
        [Display(Name = "Nombre")]
        public string FirstName { get; set; } = null!;
        [Display(Name = "Apellido")]
        public string LastName { get; set; } = null!;
        [Display(Name = "Correo electrónico")]
        public string? Email { get; set; }
        [Display(Name = "Teléfono")]
        public string? Phone { get; set; }

        public virtual List<Address> Addresses { get; set; }
    }
}
