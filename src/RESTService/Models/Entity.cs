using RESTService.Links;
using System.Runtime.Serialization;

namespace RESTService.Models
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class Entity
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Resources Resources { get; private set; } = new Resources();
    }
}