using System.ComponentModel.DataAnnotations;
using RecruitmentAgency.Models;

namespace RecruitmentAgency.Models.Identity
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Роль")]
        public int UserRole { get; set; }
    }
}