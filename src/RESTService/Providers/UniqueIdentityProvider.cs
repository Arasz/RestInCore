using MongoDB.Driver;
using RESTService.Database;
using RESTService.Models;
using System.Runtime.Serialization;

namespace RESTService.Providers
{
    [DataContract]
    public class UniqueIdentityProvider<T> : IIdentityProvider<T>
        where T : Entity
    {
        private readonly IMongoCollection<TypeIdentity> _typeIdentitiesCollection;
        private readonly TypeIdentity _typeIdentity;
        private string _entityName = typeof(T).Name;
        private FilterDefinition<TypeIdentity> _filter;

        [DataMember]
        private int _lastId = 1;

        public int Id
        {
            get
            {
                _typeIdentity.LastUsedId = _lastId;
                _typeIdentitiesCollection.ReplaceOne(_filter, _typeIdentity);
                return _lastId++;
            }
        }

        public UniqueIdentityProvider(MongoDbManager manager)
        {
            _filter = new FilterDefinitionBuilder<TypeIdentity>().Where(identity => identity.Id == _entityName);
            _typeIdentitiesCollection = manager.DefaultDatabase.GetCollection<TypeIdentity>("typeIdentitiesCollection");
            var firstElement = _typeIdentitiesCollection.Find(identity => identity.Id == _entityName).FirstOrDefault();
            _typeIdentity = new TypeIdentity() { LastUsedId = _lastId, Id = _entityName };
            if (firstElement == null)
                _typeIdentitiesCollection.InsertOne(_typeIdentity);
            else
                _lastId = firstElement.LastUsedId;
        }
    }
}