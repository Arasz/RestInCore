using System;
using RESTService.Models;
using System.Collections.Generic;
using System.Linq;

namespace RESTService.Repository
{

    public class StudentsRepository : IRepository<Student>
    {
        private IList<Student> _students;

        public StudentsRepository()
        {
            _students = new List<Student>
            {
                new Student(){Birthday = DateTime.Today, Name = "Andrzej", Surname = "Tkacz"},
                new Student(){Birthday = DateTime.Now, Name = "Kalina", Surname = "Wilk"}
            };
        }

        public void Create(Student newItem) => _students.Add(newItem);

        public void Delete(Student item) => _students.Remove(item);

        public Student Retrive(int index) => _students.ElementAt(index);

        public IEnumerable<Student> RetriveAll() => _students; 

        public void Update(int index, Student newItem) => _students[index] = newItem;
    }
}