using RESTService.Links;
using RESTService.Providers;
using System.Runtime.Serialization;

namespace RESTService.Models
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class Entity
    {
        private static IIdentityProvider<int> _identityProvider;

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Resources Resources { get; }

        public Entity(IIdentityProvider<int> identityProvider, bool changeProvider = false)
        {
            if (_identityProvider == null || changeProvider)
                _identityProvider = identityProvider;

            Id = _identityProvider?.Id ?? 0;
        }
    }
}