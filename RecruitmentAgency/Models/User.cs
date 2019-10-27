using FluentNHibernate.Mapping;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace RecruitmentAgency.Models
{
    public class User : IUser<int>
    {
        [HiddenInput(DisplayValue = false)]
        public virtual int Id { get; protected set; }

        [Display(Name = "Логин")]
        [Required]
        public virtual string UserName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public virtual string PasswordHash { get; set; }

        [Display(Name = "Роль")]
        [Required]
        public virtual UserRole UserRole { get; set; }

        public class Map : ClassMap<User>
        {
            public Map()
            {
                Table("Users");
                Id(x => x.Id).GeneratedBy.Identity();
                Map(x => x.UserName).Not.Nullable().Unique();
                Map(x => x.PasswordHash).Not.Nullable();
                References(x => x.UserRole).Column("UserRoleId").Not.Nullable();
            }
        }
    }
}