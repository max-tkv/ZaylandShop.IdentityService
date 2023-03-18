using System.ComponentModel.DataAnnotations;

namespace ZaylandShop.IdentityService.Controllers.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
       
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string ReturnUrl { get; set; }

        [Required]
        public string State { get; set; }
    }
}
