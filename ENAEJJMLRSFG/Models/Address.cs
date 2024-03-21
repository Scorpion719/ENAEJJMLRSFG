using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ENAEJJMLRSFG.Models
{
    public partial class Address
    {
        public int Id { get; set; }

        [Display(Name = "Cliente")]
        public int CustomerId { get; set; }
        [Display(Name = "Calle")]
        public string Street { get; set; } = null!;
        [Display(Name = "Ciudad")]
        public string City { get; set; } = null!;
        [Display(Name = "Estado")]
        public string State { get; set; } = null!;
        [Display(Name = "País")]
        public string Country { get; set; } = null!;
        [Display(Name = "Código postal")]
        public string? PostalCode { get; set; }

        public virtual Customer Customer { get; set; } = null!;
    }
}
