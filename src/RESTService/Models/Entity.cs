using MongoDB.Bson.Serialization.Attributes;
using RESTService.Links;
using System.Runtime.Serialization;

namespace RESTService.Models
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class Entity
    {
        public Entity()
        {
            Resources = new Resources();
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember, BsonIgnore]
        public Resources Resources { get; set; }
    }
}