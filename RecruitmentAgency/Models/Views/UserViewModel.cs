using System.ComponentModel.DataAnnotations;

namespace RecruitmentAgency.Models.Views
{
    public class UserViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string UserName { get; set; }
        
        [Required]
        [Display(Name = "Роль")]
        public int UserRole { get; set; }
    }
}