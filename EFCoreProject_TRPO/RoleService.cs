using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreProject_TRPO
{
    public class RoleService
    {
        private readonly AppDbContext _db = BaseDbService.Instance.Context;

        public ObservableCollection<Role> Roles { get; set; } = new();

        public RoleService()
        {
            GetAll();
        }

        public void GetAll()
        {
            var roles = _db.Roles.ToList();
            Roles.Clear();
            foreach (var role in roles)
            {
                Roles.Add(role);
            }
        }

        public int Commit() => _db.SaveChanges();

        public void Add(Role role)
        {
            var _role = new Role
            {
                Title = role.Title,
            };
            _db.Add<Role>(_role);
            Commit();
            Roles.Add(_role);
        }

        public void Remove(Role role)
        {
            _db.Remove<Role>(role);
            if (Commit() > 0)
                if (Roles.Contains(role))
                    Roles.Remove(role);
        }

        public void Update(Role role)
        {
            Commit();
        }
    }
}
