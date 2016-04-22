using System.Runtime.Serialization;
using Microsoft.AspNet.Mvc;
using RESTService.Providers;

namespace RESTService.Models
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class Entity
    {
        [DataMember]
        public int Id { get; private set; }

        public Entity(IIdentityProvider<int> identityProvider )
        {
            Id = identityProvider.Id;
        }
    }
}