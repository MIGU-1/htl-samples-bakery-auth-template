using System;
using System.ComponentModel.DataAnnotations;

namespace Bakery.Core.DTOs
{
    public class CustomerWithDetailsDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Firstname { get; set; }
        [Required]
        [MaxLength(20)]
        public string Lastname { get; set; }

        public string UserName { get; set; }
        public DateTime RegisteredSince { get; set; }
        public bool IsAdmin { get; set; }
    }
}
