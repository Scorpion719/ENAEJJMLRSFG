﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENAEJJMLRSFG.Models
{
    public partial class User
    {
        public int Id { get; set; }
        [Display(Name = "Nombre de usuario")]
        [StringLength(50
            )]
        public string UserName { get; set; } = null!;
        [Display(Name = "Contraseña")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
        public string Password { get; set; } = null!;
        [Display(Name = "Correo electrónico")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Display(Name = "Estado")]
        public byte Status { get; set; }
        [Display(Name = "Imagen")]
        public byte[]? Image { get; set; }
        [Display(Name = "Papel")]
        public int RoleId { get; set; }
        [NotMapped] // Esta propiedad no será mapeada a la base de datos
        [Compare("Password", ErrorMessage = "La confirmación de contraseña no coincide.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
        [DataType(DataType.Password)]
        public string ConfirmarPassword { get; set; }
        [NotMapped]
        public int Take { get; set; }
        public virtual Role Role { get; set; } = null!;
    }
}
