using Raven.Client.Documents;


namespace AgDataCodingAssignment.Persistence
{
    public interface IDocumentStoreLifecycle
    {
        IDocumentStore Store { get; }

        void Dispose();
    }

    public class DocumentStoreLifecycle : IDisposable, IDocumentStoreLifecycle
    {
        public IDocumentStore Store { get; private set; }
        public DocumentStoreLifecycle(string url,string databaseName)
        {
            Store = new DocumentStore
            {
                //Urls = new[] { "http://localhost:8080" },
                Urls = new[] { url },
                Database = databaseName
            }.Initialize();


        }
        public void Dispose()
        {
            Store.Dispose();
        }
    }
}
