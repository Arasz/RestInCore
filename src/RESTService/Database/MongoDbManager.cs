using MongoDB.Driver;

namespace RESTService.Database
{
    public class MongoDbManager
    {
        public IMongoClient Client { get; }

        public string ConnectionUrl => $"mongodb://{ServerHostName}:{ServerPort}";

        public IMongoDatabase DefaultDatabase { get; }

        public string DefaultDatabaseName => @"universityBase";

        public string ServerHostName { get; set; } = @"localhost";

        public int ServerPort { get; set; } = 8004;

        public MongoDbManager()
        {
            Client = new MongoClient(ConnectionUrl);
            DefaultDatabase = Client.GetDatabase(DefaultDatabaseName);
        }
    }
}