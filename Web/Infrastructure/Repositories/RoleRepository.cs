using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Configuration;
using Core.Domain;
using Glimpse.Core.Extensions;
using MongoDB.Driver;
using Web.Infrastructure.Extensions;

namespace Web.Infrastructure.Repositories
{
    public class RoleRepository:MongoRepository,IRoleRepository
    {
        readonly IMongoCollection<Role> _collection;
        internal string CollectionName { get; set; }

        public RoleRepository()
        {
            CollectionName = new Role().GetType().Name;
            _collection = Database.GetCollection<Role>(CollectionName);
        }

        public Task<Role> GetRoleAsync(RoleName roleName)
        {
            return _collection.Find(r=> r.RoleName == roleName).SingleOrDefaultAsync();
        }

        public async Task CreateRoles()
        {
            await _collection.InsertOneAsync(new Role
            {
                ApplicationName = AppDeployment.Instance.AppSetting("ApplicationName", "Monitorr"),
                RoleName = RoleName.Admin
            });

            await _collection.InsertOneAsync(new Role
            {
                ApplicationName = AppDeployment.Instance.AppSetting("ApplicationName", "Monitorr"),
                RoleName = RoleName.Enterprise
            });

            await _collection.InsertOneAsync(new Role
            {
                ApplicationName = AppDeployment.Instance.AppSetting("ApplicationName", "Monitorr"),
                RoleName = RoleName.Pro
            });

            await _collection.InsertOneAsync(new Role
            {
                ApplicationName = AppDeployment.Instance.AppSetting("ApplicationName", "Monitorr"),
                RoleName = RoleName.Free
            });

            await _collection.InsertOneAsync(new Role
            {
                ApplicationName = AppDeployment.Instance.AppSetting("ApplicationName", "Monitorr"),
                RoleName = RoleName.LogAccess
            });
        }

        public List<Role> GetAll()
        {
            return _collection.Find(x => x.ApplicationName.Length > 1).ToList();
        }

        public void Insert(Role x)
        {
            _collection.InsertOne(x);
        }
    }

    public interface IRoleRepository
    {
        Task<Role> GetRoleAsync(RoleName roleName);
        Task CreateRoles();
        List<Role> GetAll();

        void Insert(Role x);
    }
}