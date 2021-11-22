using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PostNotifier.Models
{
    public class Subscription
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Email { get; set; }
    }
}
