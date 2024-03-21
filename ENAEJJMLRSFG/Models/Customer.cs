﻿using System;
using System.Collections.Generic;

namespace ENAEJJMLRSFG.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Addresses = new HashSet<Address>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}