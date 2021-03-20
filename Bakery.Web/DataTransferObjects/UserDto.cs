using System.ComponentModel.DataAnnotations;

namespace Bakery.Web.DataTransferObjects
{
    public class UserDto : CredentialDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "The first name must be at least two characters long")]
        public string Firstname { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "The last name must be at least two characters long")]
        public string Lastname { get; set; }
    }
}
