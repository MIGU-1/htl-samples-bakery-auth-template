using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bakery.Web.DataTransferObjects
{
    public class CustomerDto
    {
        public int Id { get; set; }

        [DisplayName("Vorname")]
        [MaxLength(20, ErrorMessage = "Der {0} darf max. aus {1} Zeichen bestehen!")]
        [MinLength(2, ErrorMessage = "Der {0} muss mind. aus {1} Zeichen bestehen!")]
        public string Firstname { get; set; }

        [DisplayName("Nachname")]
        [MaxLength(20, ErrorMessage = "Der {0} darf max. aus {1} Zeichen bestehen!")]
        [MinLength(2, ErrorMessage = "Der {0} muss mind. aus {1} Zeichen bestehen!")]
        public string Lastname { get; set; }

        [EmailAddress]
        public string Username { get; set; }

        public string Name => $"{Lastname} {Firstname}";
    }
}
