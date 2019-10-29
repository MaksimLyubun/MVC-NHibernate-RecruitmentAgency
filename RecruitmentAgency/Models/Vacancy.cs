using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using FluentNHibernate.Mapping;

using RecruitmentAgency.Interfaces;

namespace RecruitmentAgency.Models
{
    public class Vacancy : IEntity
    {
        [HiddenInput(DisplayValue = false)]
        public virtual int Id { get; protected set; }

        [Display(Name = "Название")]
        [Required]
        public virtual string Name { get; set; }

        [Display(Name = "Описание")]
        public virtual string Description { get; set; }

        [Display(Name = "Срок (лет)")]
        [Range(1, 500, ErrorMessage = "Срок контракта должен быть в интервале от 1 до 500")]
        public virtual int? Term { get; set; }

        [Display(Name = "Компания")]
        [Required]
        public virtual string Company { get; set; }

        [Display(Name = "Минимальный стаж (лет)")]
        [Required]
        [Range(1, 500, ErrorMessage = "Требуемый стаж должен быть в интервале от 1 до 500")]
        public virtual int MinExperience { get; set; }

        [Display(Name = "Зарплата")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Соискателю не понравится доплачивать за работу, установите положительную зарплату")]
        public virtual int Salary { get; set; }
        
        [Display(Name = "Статус")]
        [UIHint("_vacancyArchived")]
        [Required]
        public virtual bool Archived { get; set; }
        
        public virtual int UserId { get; set; }

        public virtual User User { get; set; }

        public class Map : ClassMap<Vacancy>
        {
            public Map()
            {
                Table("Vacancies");
                Id(x => x.Id).GeneratedBy.Identity();
                Map(x => x.Name).Column("Name").Not.Nullable();
                Map(x => x.Description).Column("Description").Nullable();
                Map(x => x.Term).Column("Term").Nullable();
                Map(x => x.Company).Column("Company").Not.Nullable();
                Map(x => x.MinExperience).Column("MinExperience").Not.Nullable();
                Map(x => x.Salary).Column("Salary").Not.Nullable();
                Map(x => x.Archived).Column("Archived").Not.Nullable();
                Map(x => x.UserId).Column("UserId").Not.Nullable();
                References(x => x.User).Column("UserId").ReadOnly();
            }
        }
    }
}
