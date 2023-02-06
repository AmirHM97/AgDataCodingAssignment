using AgDataCodingAssignment.Application.Models.Dtos;
using AgDataCodingAssignment.Domain.Entities;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AgDataCodingAssignment.Persistence.Repositories
{
    public interface IRavenDbRepository
    {
        Task<UserDocument> AddAsync(UserDocument entity);
        Task Delete(string Name);
        Task<UserDocument> GetByIdAsync(string id);
        Task<UserDocument> GetByNameAsync(string name);
        Task<UserDocument> UpdateAsync(UpdateUserDto entity);
    }

    public class RavenDbRepository : IRavenDbRepository
    {
        private readonly IDocumentStore _store;
        private readonly IDocumentStoreLifecycle _storeLifecycle;
        private readonly string _collectionName;

        public RavenDbRepository(IDocumentStore documentStore)
        {
            _collectionName = "UserCollection";
            _store = documentStore;

            var result = _store.Maintenance.Server.Send(new GetDatabaseRecordOperation(_store.Database));
            if (result is null)
            {
                documentStore.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(_store.Database)));
            }
        }
     
        public async Task<UserDocument> GetByIdAsync(string id)
        {
            using IAsyncDocumentSession session = _store.OpenAsyncSession();
            return await session.LoadAsync<UserDocument>(id);
        }
        public async Task<UserDocument> GetByNameAsync(string name)
        {
            using (IAsyncDocumentSession session = _store.OpenAsyncSession())
            {
             return  await session.Query<UserDocument>().Where(w=>w.Name== name).FirstOrDefaultAsync();
            }
        }
        public async Task<UserDocument> AddAsync(UserDocument entity)
        {
            using (IAsyncDocumentSession session = _store.OpenAsyncSession())
            {
                await session.StoreAsync(entity, _collectionName + "/");
                await session.SaveChangesAsync();
                return entity;
            }
        }
        public async Task<UserDocument> UpdateAsync(UpdateUserDto entity)
        {
            using (IAsyncDocumentSession session = _store.OpenAsyncSession())
            {
                var user = await session.Query<UserDocument>().Where(w => w.Name == entity.Name).FirstOrDefaultAsync();
                user.Address = entity.Address;
                user.LastUpdatedDate= DateTimeOffset.UtcNow;
                //await session.StoreAsync(entity, _collectionName + "/");
                await session.SaveChangesAsync();
                return user;
            }
        }

        public async Task Delete(string Name)
        {
                using (IAsyncDocumentSession session = _store.OpenAsyncSession())
                {
                    var user = await session.Query<UserDocument>().Where(w => w.Name == Name).FirstOrDefaultAsync();
                    session.Delete(user.Id);
                    await session.SaveChangesAsync();
                }
          
        }
    }
}

