using RESTService.Providers;
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

        public Entity(IIdentityProvider<int> identityProvider)
        {
            Id = identityProvider?.Id ?? 0;
        }
    }
}