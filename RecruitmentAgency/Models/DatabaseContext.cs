using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNet.Identity;
using NHibernate;
using RecruitmentAgency.Models.Identity;
using System.Configuration;

namespace RecruitmentAgency.Models
{
    public class DatabaseContext
    {
        private readonly ISessionFactory sessionFactory;

        public DatabaseContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<User>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserRole>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Summary>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Vacancy>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DatabaseContext>())
                .BuildSessionFactory();
        }
        public ISession MakeSession()
        {
            return sessionFactory.OpenSession();
        }

        public IUserStore<User, int> Users
        {
            get { return new IdentityStore(MakeSession()); }
        }
    }
}