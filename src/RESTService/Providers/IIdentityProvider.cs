namespace RESTService.Providers
{
    /// <summary>
    /// Provides client with identity 
    /// </summary>
    /// <typeparam name="T"> Identity type </typeparam>
    public interface IIdentityProvider<out T>
    {
        /// <summary>
        /// Unique identity 
        /// </summary>
        T Id { get; }
    }
}