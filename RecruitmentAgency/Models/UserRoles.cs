using FluentNHibernate.Mapping;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

using RecruitmentAgency.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentAgency.Models
{
    public class UserRoles : IRole<int>
    {
        public virtual int Id { get; protected set; }

        [Display(Name = "Роль")]
        public virtual string Name { get; set; }

        public virtual IList<Users> Users { get; set; }

        public class Map : ClassMap<UserRoles>
        {
            public Map()
            {
                Id(x => x.Id);
                Map(x => x.Name).Not.Nullable();
                HasMany(x => x.Users).Inverse().Cascade.All().KeyColumn("UserRole");
            }
        }
    }
}
