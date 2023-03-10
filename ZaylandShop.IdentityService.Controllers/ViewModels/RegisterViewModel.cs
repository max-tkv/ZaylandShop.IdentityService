using System.ComponentModel.DataAnnotations;

namespace ZaylandShop.IdentityService.Controllers.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Url)]
        public string ReturnUrl { get; set; }
    }
}
