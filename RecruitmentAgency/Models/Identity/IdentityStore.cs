using Microsoft.AspNet.Identity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecruitmentAgency.Models.Identity
{
    public class IdentityStore : IUserStore<Users, int>,
        IUserPasswordStore<Users, int>,
        IUserLockoutStore<Users, int>,
        IUserTwoFactorStore<Users, int>
    {
        private readonly ISession session;

        public IdentityStore(ISession session)
        {
            this.session = session;
        }
        
        #region IUserStore<Users, int>
        public Task CreateAsync(Users user)
        {
            return Task.Run(() => session.SaveOrUpdate(user));
        }

        public Task DeleteAsync(Users user)
        {
            return Task.Run(() => session.Delete(user));
        }

        Task<Users> IUserStore<Users, int>.FindByIdAsync(int userId)
        {
            return Task.Run(() => session.Get<Users>(userId));
        }

        Task<Users> IUserStore<Users, int>.FindByNameAsync(string userName)
        {
            return Task.Run(() =>
            {
                return session.QueryOver<Users>()
                    .Where(u => u.UserName == userName)
                    .SingleOrDefault();
            });
        }

        public Task UpdateAsync(Users user)
        {
            return Task.Run(() => session.SaveOrUpdate(user));
        }
        #endregion

        #region IUserPasswordStore<User, int>
        public Task SetPasswordHashAsync(Users user, string passwordHash)
        {
            return Task.Run(() => user.PasswordHash = passwordHash);
        }

        public Task<string> GetPasswordHashAsync(Users user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(Users user)
        {
            return Task.FromResult(true);
        }
        #endregion

        #region IUserLockoutStore<User, int>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(Users user)
        {
            return Task.FromResult(DateTimeOffset.MaxValue);
        }

        public Task SetLockoutEndDateAsync(Users user, DateTimeOffset lockoutEnd)
        {
            return Task.CompletedTask;
        }

        public Task<int> IncrementAccessFailedCountAsync(Users user)
        {
            return Task.FromResult(0);
        }

        public Task ResetAccessFailedCountAsync(Users user)
        {
            return Task.CompletedTask;
        }

        public Task<int> GetAccessFailedCountAsync(Users user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(Users user)
        {
            return Task.FromResult(false);
        }

        public Task SetLockoutEnabledAsync(Users user, bool enabled)
        {
            return Task.CompletedTask;
        }
        #endregion

        #region IUserTwoFactorStore<Users, int>
        public Task SetTwoFactorEnabledAsync(Users user, bool enabled)
        {
            return Task.CompletedTask;
        }

        public Task<bool> GetTwoFactorEnabledAsync(Users user)
        {
            return Task.FromResult(false);
        }
        #endregion

        public void Dispose()
        {
        }
    }
}