using System.ComponentModel.DataAnnotations;

namespace ETICARET.WEBUI.Models
{
    public class RegisterModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Şifreler uyuşmuyor.")]
        public string RePassword { get; set; }
    }
}
