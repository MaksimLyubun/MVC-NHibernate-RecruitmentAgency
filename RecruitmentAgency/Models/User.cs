﻿using FluentNHibernate.Mapping;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using RecruitmentAgency.Interfaces;

namespace RecruitmentAgency.Models
{
    public class User : IUser<int>, IEntity
    {
        const string admin = "Администратор";
        const string jobseeker = "Соискатель";
        const string employee = "Работодатель";
        
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

        public virtual bool IsAdmin()
        {
            return UserRole.Name == admin;
        }

        public virtual bool IsJobseeker()
        {
            return UserRole.Name == jobseeker;
        }

        public virtual bool IsEmployee()
        {
            return UserRole.Name == employee;
        }

        public class Map : ClassMap<User>
        {
            public Map()
            {
                Table("Users");
                Id(x => x.Id).GeneratedBy.Identity();
                Map(x => x.UserName).Column("UserName").Not.Nullable().Unique();
                Map(x => x.PasswordHash).Column("PasswordHash").Not.Nullable();
                References(x => x.UserRole).Column("UserRoleId").Not.Nullable();
            }
        }
    }
}