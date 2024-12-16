using System.ComponentModel.DataAnnotations;

namespace AccountService.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
