using RESTService.Models;

namespace RESTService.Services
{
    public interface IIdentityAssignService<T>
        where T : Entity
    {
        void AssignIdentity(T entity);
    }
}