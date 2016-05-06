using RESTService.Models;

namespace RESTService.Providers
{
    /// <summary>
    /// Provides client with identity 
    /// </summary>
    /// <typeparam name="T"> Identity type </typeparam>
    public interface IIdentityProvider<T>
        where T : Entity
    {
        /// <summary>
        /// Unique identity 
        /// </summary>
        int Id { get; }
    }
}