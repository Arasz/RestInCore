using MongoDB.Driver;
using RESTService.Database;
using RESTService.Models;
using RESTService.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace RESTService.Repository
{
    public class StudentsRepository : IRepository<Student>
    {
        private readonly FilterDefinitionBuilder<Student> _filterBuilder = new FilterDefinitionBuilder<Student>();
        private readonly IIdentityAssignService<Student> _identityAssignService;
        private readonly IMongoCollection<Student> _mongoCollection;

        /// <summary>
        /// </summary>
        private MongoDbManager _databaseManager;

        public StudentsRepository(IIdentityAssignService<Student> identityAssignService, MongoDbManager databaseManager)
        {
            _identityAssignService = identityAssignService;
            _databaseManager = databaseManager;

            _mongoCollection = _databaseManager.DefaultDatabase.GetCollection<Student>("students");
        }

        public async Task Create(Student entity)
        {
            _identityAssignService.AssignIdentity(entity);
            await _mongoCollection.InsertOneAsync(entity).ConfigureAwait(false);
        }

        public async Task Delete(Student entity)
        {
            var filter = _filterBuilder.Where(student => student.Id == entity.Id);
            await _mongoCollection.DeleteOneAsync(filter).ConfigureAwait(false);
        }

        public async Task DeleteAll()
        {
            var filter = _filterBuilder.Empty;
            await _mongoCollection.DeleteManyAsync(filter).ConfigureAwait(false);
        }

        public async Task<Student> Read(int id)
        {
            var filter = _filterBuilder.Where(student => student.Id == id);
            var result = await _mongoCollection.FindAsync(filter).ConfigureAwait(false);
            return result?.First();
        }

        public async Task<IEnumerable<Student>> ReadAll()
        {
            var filter = _filterBuilder.Empty;
            var result = await _mongoCollection.FindAsync(filter).ConfigureAwait(false);
            return result.ToList();
        }

        public async Task<IEnumerable<Student>> ReadMatchingStudent(Expression<Func<Student, bool>> matchExpression)
        {
            var filter = Builders<Student>.Filter.Where(matchExpression);
            return await _mongoCollection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Student>> ReadMatchingStudentByRegex(Expression<Func<Student, object>> field, string pattern)
        {
            var bsonRegex = new BsonRegularExpression(pattern);
            var filter = Builders<Student>.Filter.Regex(field, bsonRegex);
            return await _mongoCollection.Find(filter).ToListAsync();
        }

        public async Task Update(Student entity) => await Update(entity.Id, entity).ConfigureAwait(false);

        public async Task Update(int id, Student entity)
        {
            var filter = _filterBuilder.Where(student => student.Id == id);
            var update = Builders<Student>.Update
                .Set(student => student.Birthday, entity.Birthday)
                .Set(student => student.Name, entity.Name)
                .Set(student => student.Surname, entity.Surname);
            await _mongoCollection.UpdateOneAsync(filter, update).ConfigureAwait(false);
        }
    }
}