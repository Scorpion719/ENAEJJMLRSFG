using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ENAEJJMLRSFG.Models
{
    public partial class Address
    {
        
        public int Id { get; set; }

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "El campo Cliente")]
        
        public int CustomerId { get; set; }
        [Display(Name = "Calle")]
        [Required(ErrorMessage = "El campo Calle")]
        [StringLength(100)]
        public string Street { get; set; } = null!;
        [Display(Name = "Ciudad")]
        [Required(ErrorMessage = "El campo Ciudad")]
        [StringLength(50)]
        public string City { get; set; } = null!;
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo Estado")]
        [StringLength(50)]
        public string State { get; set; } = null!;
        [Display(Name = "País")]
        [Required(ErrorMessage = "El campo País")]
        [StringLength(50)]
        public string Country { get; set; } = null!;
        [Display(Name = "Código postal")]
        [Required(ErrorMessage = "El campo postal")]
        [StringLength(20)]
        public string? PostalCode { get; set; }

        public virtual Customer Customer { get; set; } = null!;
    }
}
