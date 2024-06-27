using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace InstantaneousGram_ImageAndVideoProcessing.Data
{
    public class CosmosDbContext
    {
        public CosmosClient Client { get; }
        public Database Database { get; }
        public Container Container { get; }

        public CosmosDbContext(IConfiguration configuration)
        {
            string connectionString = configuration["CosmosDb:ConnectionString"];
            string databaseName = configuration["CosmosDb:DatabaseName"];
            string containerName = configuration["CosmosDb:ContainerName"];

            Client = new CosmosClient(connectionString);
            Database = Client.GetDatabase(databaseName);
            Container = Database.GetContainer(containerName);
        }
    }
}
