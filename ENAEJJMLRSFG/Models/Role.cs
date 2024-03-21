using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ENAEJJMLRSFG.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;
        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
