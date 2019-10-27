using System.ComponentModel.DataAnnotations;

namespace RecruitmentAgency.Models.Views
{
    public class LoginViewModel
    {
        [Display(Name = "Логин")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}