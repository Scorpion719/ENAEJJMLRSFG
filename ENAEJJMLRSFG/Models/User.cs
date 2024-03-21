using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ENAEJJMLRSFG.Models
{
    public partial class User
    {
        public int Id { get; set; }
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; } = null!;
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = null!;
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; } = null!;
        [Display(Name = "Estado")]
        public byte Status { get; set; }
        [Display(Name = "Imagen")]
        public byte[]? Image { get; set; }
        [Display(Name = "Papel")]
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}
