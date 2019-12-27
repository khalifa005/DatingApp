using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(15,MinimumLength=8,ErrorMessage="max password 15 minlength8")]
        public string  Password { get; set; }
    }
}