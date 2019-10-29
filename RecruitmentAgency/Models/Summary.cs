using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;

using RecruitmentAgency.Interfaces;

namespace RecruitmentAgency.Models
{
    public class Summary : IEntity
    {
        [HiddenInput(DisplayValue = false)]
        public virtual int Id { get; protected set; }

        [Display(Name = "ФИО")]
        [Required]
        public virtual string JobseekerName { get; set; }

        [Display(Name = "Дата рождения")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy'-'MM'-'dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime DateOfBirth { get; set; } = DateTime.Now;

        [Display(Name = "Стаж (количество лет)")]
        [Required]
        [Range(1, 500, ErrorMessage = "Стаж должен быть в интервале от 1 до 500")]
        public virtual int Experience { get; set; } = 0;

        [Display(Name = "Фотография")]
        [HiddenInput(DisplayValue = false)]
        public virtual byte[] Photo { get; set; }

        [HiddenInput(DisplayValue = false)]

        public virtual int UserId { get; set; }

        public virtual User User { get; set; }

        public class Map : ClassMap<Summary>
        {
            public Map()
            {
                Table("Summaries");
                Id(x => x.Id).GeneratedBy.Identity();
                Map(x => x.JobseekerName).Column("JobseekerName").Not.Nullable();
                Map(x => x.DateOfBirth).Column("DateOfBirth").Not.Nullable();
                Map(x => x.Experience).Column("Experience").Not.Nullable();
                Map(x => x.Photo).Column("Photo").CustomSqlType("VARBINARY (MAX)").Length(Int32.MaxValue).Nullable();
                Map(x => x.UserId).Column("UserId").Not.Nullable();
                References(x => x.User).Column("UserId").ReadOnly();
            }
        }
    }
}
