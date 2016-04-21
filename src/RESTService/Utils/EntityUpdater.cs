using System.Reflection;

namespace RESTService.Utils
{
    /// <summary>
    /// Updates entity state with new state 
    /// </summary>
    /// <typeparam name="T"> Entity type </typeparam>
    public class EntityUpdater<T> where T : class
    {
        private readonly PropertyInfo[] _allEntityProperties;
        private readonly T _entity;

        public EntityUpdater(T entity)
        {
            _entity = entity;

            _allEntityProperties = _entity.GetType().GetProperties();
        }

        /// <summary>
        /// Updates all properties of entity with new values from new entity 
        /// </summary>
        /// <param name="newEntity"> Entity from which new values will be taken </param>
        public void Update(T newEntity)
        {
            foreach (var propertyInfo in _allEntityProperties)
            {
                propertyInfo.SetValue(_entity, propertyInfo.GetValue(newEntity));
            }
        }
    }
}