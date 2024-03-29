using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ScWorks.Api.Data.Models
{
    public class Work
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }
    }
}
