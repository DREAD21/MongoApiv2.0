using MongoApi.Models;
using MongoDB.Driver;

namespace MongoApi
{
    public class MongoDbManager
    {
        private readonly IMongoClient _client;

        public MongoDbManager(IMongoClient client)
        {
            _client = client;
        }

        public void SaveFile(string databaseName, string key, byte[] content)
        {
            var database = _client.GetDatabase(databaseName);
            var collection = database.GetCollection<FileModel>("files");

            var fileModel = new FileModel
            {
                Key = key,
                Content = content
            };

            collection.InsertOne(fileModel);
        }

        public byte[] GetFile(string databaseName, string key)
        {
            var database = _client.GetDatabase(databaseName);
            var collection = database.GetCollection<FileModel>("files");

            var filter = Builders<FileModel>.Filter.Eq("Key", key);
            var fileModel = collection.Find(filter).FirstOrDefault();

            return fileModel?.Content;
        }
    }
}
