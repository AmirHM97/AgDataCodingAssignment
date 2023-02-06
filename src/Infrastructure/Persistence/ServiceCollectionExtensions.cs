using AgDataCodingAssignment.Application.Contracts;
using AgDataCodingAssignment.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Extensions;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Session;
using Raven.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.WebRequestMethods;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide;

namespace AgDataCodingAssignment.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            //services.AddSingleton(sp =>
            //{
            //    var documentStore = new DocumentStore
            //    {
            //        Urls = new[] { "http://live-test.ravendb.net/ " },
            //        Database = "MyDatabase"
            //    };
            //    documentStore.Initialize();
            //    var result = documentStore.Maintenance.Server.Send(new GetDatabaseRecordOperation("MyDatabase"));
            //    if (result is null)
            //    {
            //        documentStore.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord("MyDatabase")));
            //    }
            //    return documentStore;
            //});
            services.AddRavenDbDocStore();

            // 2. Add a scoped IAsyncDocumentSession. For the sync version, use .AddRavenSession().
            services.AddRavenDbAsyncSession();
            //services.AddSingleton<IDocumentStoreLifecycle,DocumentStoreLifecycle>(
            //    sp => {
            //        return new DocumentStoreLifecycle(connectionString, );
            //        }
            //    );
            services.AddMemoryCache();

            services.AddScoped<IMemoryCacheRepository, MemoryCacheRepository>();
            services.AddScoped<IRavenDbRepository, RavenDbRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
