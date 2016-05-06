using RESTService.Models;
using RESTService.Providers;

namespace RESTService.Services
{
    public class IdentityAssignService<T> : IIdentityAssignService<T>
        where T : Entity
    {
        /// <summary>
        /// </summary>
        private readonly IIdentityProvider<T> _identityProvider;

        public IdentityAssignService(IIdentityProvider<T> identityProvider)
        {
            _identityProvider = identityProvider;
        }

        public void AssignIdentity(T entity)
        {
            entity.Id = _identityProvider.Id;
        }
    }
}