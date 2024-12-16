using System.ComponentModel.DataAnnotations;

namespace AccountService.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
