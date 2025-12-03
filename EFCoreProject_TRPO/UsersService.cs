using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EFCoreProject_TRPO
{
    public class UsersService
    {
        private readonly AppDbContext _db = BaseDbService.Instance.Context;

        public ObservableCollection<User> Users { get; set; } = new();

        public UsersService()
        {
            GetAll();
        }

        public bool IsLoginUnique(string login, int? currentUserId = null)
        {
            return !_db.Users.Any(u =>
                u.Login.ToLower() == login.ToLower() &&
                u.Id != (currentUserId ?? -1));
        }

        public bool IsEmailUnique(string email, int? currentUserId = null)
        {
            return !_db.Users.Any(u =>
                u.Email.ToLower() == email.ToLower() &&
                u.Id != (currentUserId ?? -1));
        }

        public void Add(User user)
        {
            var _user = new User
            {
                Login = user.Login,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                CreatedAt = user.CreatedAt,
                RoleId = user.RoleId,
                Profile = user.Profile ?? new UserProfile()
            };

            if (_user.Profile != null && _user.Profile.AvatarUrl == null && _user.Profile.Bio == null && _user.Profile.Phone == null)
            {
                _user.Profile.AvatarUrl = "";
                _user.Profile.Bio = "";
                _user.Profile.Phone = "";
            }

            _db.Add<User>(_user);
            Commit();

            _db.Entry(_user).Reference(u => u.Role).Load();
            _db.Entry(_user).Reference(u => u.Profile).Load();

            Users.Add(_user);
        }

        public int Commit() => _db.SaveChanges();

        public void GetAll()
        {
            var users = _db.Users
                .Include(u => u.Role)
                .Include(u => u.Profile)
                .ToList();

            Users.Clear();
            foreach (var user in users)
            {
                if (user.Profile == null)
                {
                    user.Profile = new UserProfile();
                }
                Users.Add(user);
            }
        }

        public void Remove(User user)
        {
            _db.Remove<User>(user);
            if (Commit() > 0)
                if (Users.Contains(user))
                    Users.Remove(user);
        }

        public void Update(User user)
        {
            Commit();
        }
    }
}
