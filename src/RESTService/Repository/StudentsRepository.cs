using RESTService.Models;
using RESTService.Providers;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RESTService.Repository
{
    /// <summary>
    /// Repository containing Students 
    /// </summary>
    [DataContract]
    public class StudentsRepository : IRepository<Student>
    {
        [DataMember]
        private readonly UniqueIdProvider _idProvider;

        /// <summary>
        /// All students collection 
        /// </summary>
        [DataMember]
        private readonly Dictionary<int, Student> _students;

        public StudentsRepository(UniqueIdProvider idProvider)
        {
            _idProvider = idProvider;

            IList<int> ids = new List<int>();

            for (int i = 0; i < 3; i++)
                ids.Add(_idProvider.Id);

            _students = new Dictionary<int, Student>
            {
                [ids[0]] = new Student() { Birthday = DateTime.Today, Name = "Andrzej", Surname = "Tkacz", Id = ids[0] },
                [ids[1]] = new Student() { Birthday = DateTime.Today, Name = "Przemysław", Surname = "Kaleta", Id = ids[1] },
                [ids[2]] = new Student() { Birthday = DateTime.Now, Name = "Kalina", Surname = "Wilk", Id = ids[2] }
            };
        }

        public void Create(Student item)
        {
            var studentId = item.Id;

            var hasStudent = _students.ContainsKey(studentId);

            if (hasStudent && _students[studentId].Equals(item))
                return;

            if (hasStudent)
            {
                item.Id = _idProvider.Id;
                _students[item.Id] = item;
            }

            _students[item.Id] = item;
        }

        /// <exception cref="KeyNotFoundException"> Student with given id don't exist </exception>
        public void Delete(Student item)
        {
            if (!_students.ContainsKey(item.Id))
                throw new KeyNotFoundException("Student with given id don't exist");
            _students.Remove(item.Id);
        }

        /// <exception cref="KeyNotFoundException"> Condition. </exception>
        public Student Read(int id)
        {
            if (!_students.ContainsKey(id))
                throw new KeyNotFoundException("Student with given id don't exist");

            return _students[id];
        }

        public IEnumerable<Student> ReadAll() => _students.Values;

        /// <exception cref="ArgumentException"> "Can't find entity in repository </exception>
        public void Update(Student item)
        {
            Update(item.Id, item);
        }

        public void Update(int id, Student item)
        {
            if (!_students.ContainsKey(id))
                throw new ArgumentException("Can't find entity in repository");

            if (id != item.Id)
                throw new ArgumentException("Given id different from student id");

            _students[id] = item;
        }
    }
}