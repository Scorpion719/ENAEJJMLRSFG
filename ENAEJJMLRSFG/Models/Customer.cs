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
        [StringLength(50)]
        [Required(ErrorMessage = "El campo Nombre")]
        public string FirstName { get; set; } = null!;
        [Display(Name = "Apellido")]
        [StringLength(50)]
        [Required(ErrorMessage = "El campo Apellido")]
        public string LastName { get; set; } = null!;
        [Display(Name = "Correo electrónico")]
        [StringLength(100)]
        [Required(ErrorMessage = "El campo Correo electrónico")]
        [EmailAddress(ErrorMessage = "El campo Email debe tener un formato válido.")]
        public string? Email { get; set; }
        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El campo Teléfono")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "El campo Teléfono debe tener entre 10 y 12 caracteres.")]
        //[RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "El campo Teléfono debe tener el formato XXX-XXX-XXXX.")]
        public string? Phone { get; set; }

        public virtual List<Address> Addresses { get; set; }
    }
}
