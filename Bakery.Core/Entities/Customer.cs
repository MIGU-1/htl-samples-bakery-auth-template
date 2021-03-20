using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bakery.Core.Entities
{
    public partial class Customer : EntityObject
    {
        [DisplayName("Kunden Nr.")]
        [Required]
        public string CustomerNr { get; set; }
        
        [DisplayName("Vorname")]
        [MaxLength(20, ErrorMessage = "Der {0} darf max. aus {1} Zeichen bestehen!")]
        [MinLength(2, ErrorMessage = "Der {0} muss mind. aus {1} Zeichen bestehen!")]
        public string Firstname { get; set; }

        [DisplayName("Nachname")]
        [MaxLength(20, ErrorMessage = "Der {0} darf max. aus {1} Zeichen bestehen!")]
        [MinLength(2, ErrorMessage = "Der {0} muss mind. aus {1} Zeichen bestehen!")]
        public string Lastname { get; set; }

        public DateTime RegisteredSince { get; set; }

        public ICollection<Order> Orders { get; set; }

        public string FullName => Firstname + " " + Lastname;

        public override string ToString() => $"{Firstname} {Lastname}";

        public Customer()
        {
            RegisteredSince = DateTime.Now;
            
            // TODO: Initialize SecurityStamp!
        }
    }
}
