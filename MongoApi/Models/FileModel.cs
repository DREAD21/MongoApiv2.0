using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoApi.Models
{
    public class FileModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Key { get; set; }

        public byte[] Content { get; set; }
    }
}
