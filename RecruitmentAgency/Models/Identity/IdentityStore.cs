using Microsoft.AspNet.Identity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecruitmentAgency.Models.Identity
{
    public class IdentityStore : IUserStore<User, int>,
        IUserPasswordStore<User, int>,
        IUserLockoutStore<User, int>,
        IUserTwoFactorStore<User, int>
    {
        private readonly ISession session;

        public IdentityStore(ISession session)
        {
            this.session = session;
        }
        
        #region IUserStore<Users, int>
        public Task CreateAsync(User user)
        {
            return Task.Run(() => session.SaveOrUpdate(user));
        }

        public Task DeleteAsync(User user)
        {
            return Task.Run(() => session.Delete(user));
        }

        Task<User> IUserStore<User, int>.FindByIdAsync(int userId)
        {
            return Task.Run(() => session.Get<User>(userId));
        }

        Task<User> IUserStore<User, int>.FindByNameAsync(string userName)
        {
            return Task.Run(() =>
            {
                return session.QueryOver<User>()
                    .Where(u => u.UserName == userName)
                    .SingleOrDefault();
            });
        }

        public Task UpdateAsync(User user)
        {
            return Task.Run(() => session.SaveOrUpdate(user));
        }
        #endregion

        #region IUserPasswordStore<User, int>
        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            return Task.Run(() => user.PasswordHash = passwordHash);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(true);
        }
        #endregion

        #region IUserLockoutStore<User, int>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return Task.FromResult(DateTimeOffset.MaxValue);
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            return Task.CompletedTask;
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            return Task.CompletedTask;
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            return Task.CompletedTask;
        }
        #endregion

        #region IUserTwoFactorStore<Users, int>
        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            return Task.CompletedTask;
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }
        #endregion

        public void Dispose()
        {
        }
    }
}