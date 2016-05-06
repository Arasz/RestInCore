using MongoDB.Driver;
using RESTService.Database;
using RESTService.Exceptions;
using RESTService.Models;
using RESTService.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTService.Repository
{
    public class SubjectsRepository : IRepository<Subject>
    {
        private readonly FilterDefinitionBuilder<Subject> _filterBuilder = new FilterDefinitionBuilder<Subject>();
        private readonly IIdentityAssignService<Subject> _identityAssignService;
        private readonly IIdentityAssignService<Mark> _markIdentityAssignService;
        private readonly IMongoCollection<Subject> _mongoCollection;

        /// <summary>
        /// </summary>
        private MongoDbManager _databaseManager;

        public SubjectsRepository(IIdentityAssignService<Subject> identityAssignService, IIdentityAssignService<Mark> markIdentityAssignService, MongoDbManager databaseManager)
        {
            _identityAssignService = identityAssignService;
            _markIdentityAssignService = markIdentityAssignService;
            _databaseManager = databaseManager;

            _mongoCollection = _databaseManager.DefaultDatabase.GetCollection<Subject>("subjects");
        }

        public async Task Create(Subject entity)
        {
            _identityAssignService.AssignIdentity(entity);
            await _mongoCollection.InsertOneAsync(entity).ConfigureAwait(false);
        }

        public async Task CreateMarkForSubject(int id, Mark mark)
        {
            var marks = await ReadAllMarksForSubject(id).ConfigureAwait(false);
            IList<Mark> marksList;
            if (marks != null)
            {
                marksList = marks.ToList();
                marksList.Add(mark);
            }
            else
            {
                marksList = new List<Mark> { mark };
            }

            await CreateMarksForSubject(id, marksList).ConfigureAwait(false);
        }

        public async Task CreateMarksForSubject(int id, IEnumerable<Mark> marks)
        {
            var marksWithIdentity = marks.Select(mark =>
            {
                if (mark.Id == 0)
                    _markIdentityAssignService.AssignIdentity(mark);
                return mark;
            }).ToList();
            await UpdateMarksForSubject(id, marksWithIdentity).ConfigureAwait(false);
        }

        public async Task Delete(Subject entity)
        {
            var filter = _filterBuilder.Where(subject => subject.Id == entity.Id);
            await _mongoCollection.DeleteOneAsync(filter).ConfigureAwait(false);
        }

        public async Task DeleteAll()
        {
            var filter = _filterBuilder.Empty;
            await _mongoCollection.DeleteManyAsync(filter).ConfigureAwait(false);
        }

        public async Task DeleteAllMarksForSubject(int id, int markId)
        {
            await UpdateMarksForSubject(id, null).ConfigureAwait(false);
        }

        /// <exception cref="DeleteMarkException"> Condition. </exception>
        public async Task DeleteMarkForSubject(int id, int markId)
        {
            var marks = await ReadAllMarksForSubject(id).ConfigureAwait(false);

            if (marks == null)
                throw new DeleteMarkException(markId, id, "No mark was found in subject");

            var markToRemove = marks.FirstOrDefault(mark => mark.Id == markId);
            if (markToRemove == null)
                throw new DeleteMarkException(markId, id, "No mark of given id was found in subject");
            var marksList = marks.ToList();
            marksList.Remove(markToRemove);
            await UpdateMarksForSubject(id, marksList).ConfigureAwait(false);
        }

        public async Task<Subject> Read(int id)
        {
            var filter = _filterBuilder.Where(subject => subject.Id == id);
            var result = await _mongoCollection.FindAsync(filter).ConfigureAwait(false);
            return result.First();
        }

        public async Task<IEnumerable<Subject>> ReadAll()
        {
            var filter = _filterBuilder.Empty;
            var result = await _mongoCollection.FindAsync(filter).ConfigureAwait(false);
            return result.ToList();
        }

        public async Task<IEnumerable<Mark>> ReadAllMarksForSubject(int id)
        {
            var projection = Builders<Subject>.Projection.Expression(subject => new { subject.Marks });
            var result = await _mongoCollection.Find(subject => subject.Id == id)
                .Project(projection).FirstOrDefaultAsync().ConfigureAwait(false);
            return result.Marks;
        }

        public async Task Update(Subject entity) => await Update(entity.Id, entity).ConfigureAwait(false);

        public async Task Update(int id, Subject entity)
        {
            var filter = Builders<Subject>.Filter.Where(subject => subject.Id == id);
            var update = Builders<Subject>.Update
                .Set(subject => subject.Marks, entity.Marks)
                .Set(subject => subject.Name, entity.Name)
                .Set(subject => subject.Teacher, entity.Teacher);
            await _mongoCollection.UpdateOneAsync(filter, update).ConfigureAwait(false);
        }

        public async Task UpdateMarksForSubject(int id, IEnumerable<Mark> marks)
        {
            var filter = Builders<Subject>.Filter.Where(subject => subject.Id == id);
            var update = Builders<Subject>.Update.Set(subject => subject.Marks, marks.ToList());
            await _mongoCollection.UpdateOneAsync(filter, update).ConfigureAwait(false);
        }
    }
}