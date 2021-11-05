using System.ComponentModel.DataAnnotations;

namespace IdentityServer.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }
        [Required]
        public string ReturnUrl { get; set; }
    }
}
