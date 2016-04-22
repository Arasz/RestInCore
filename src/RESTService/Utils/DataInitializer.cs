using RESTService.Models;
using RESTService.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RESTService.Utils
{
    /// <summary>
    /// Initializes test repository data 
    /// </summary>
    public class DataInitializer
    {
        private readonly IIdentityProvider<int> _identityProvider;
        private IList<StudentMark> _marks = new List<StudentMark>();
        private Random _random = new Random();
        private IList<Student> _students = new List<Student>();
        private IList<Subject> _subjects = new List<Subject>();
        public IEnumerable<Entity> Data { get; }

        public DataInitializer(IIdentityProvider<int> identityProvider)
        {
            _identityProvider = identityProvider;

            InitializeStudents();
            InitializeMarks();
            InitializeSubjects();

            Data = _students.Concat<Entity>(_marks).Concat(_subjects);
        }

        private DateTime GenerateRandomDate(int year = 0)
        {
            if (year == 0)
                year = _random.Next(1988, 2000);

            return DateTime.Parse($"{year}-{_random.Next(1, 13)}-{_random.Next(1, 27)}");
        }

        private Mark GenerateRandomMark()
        {
            return (Mark)_random.Next(0, 8);
        }

        private void InitializeMarks()
        {
            foreach (var student in _students)
            {
                _marks.Add(new StudentMark(student.Id, GenerateRandomDate(2012), GenerateRandomMark(), _identityProvider));
                _marks.Add(new StudentMark(student.Id, GenerateRandomDate(2012), GenerateRandomMark(), _identityProvider));
                _marks.Add(new StudentMark(student.Id, GenerateRandomDate(2012), GenerateRandomMark(), _identityProvider));
                _marks.Add(new StudentMark(student.Id, GenerateRandomDate(2012), GenerateRandomMark(), _identityProvider));
                _marks.Add(new StudentMark(student.Id, GenerateRandomDate(2012), GenerateRandomMark(), _identityProvider));
            }
        }

        private void InitializeStudents()
        {
            _students.Add(new Student("Andrzej", "Tkacz", GenerateRandomDate(), _identityProvider));
            _students.Add(new Student("Basia", "Wilk", GenerateRandomDate(), _identityProvider));
            _students.Add(new Student("Edward", "Tim", GenerateRandomDate(), _identityProvider));
            _students.Add(new Student("Miłek", "Boban", GenerateRandomDate(), _identityProvider));
        }

        private void InitializeSubjects()
        {
            _subjects.Add(new Subject("Zarządzanie kotełami", "Naczelny kotli prezes", _identityProvider));
            _subjects.Add(new Subject("Piesły w środowisku naturalnym", "Doge", _identityProvider));

            foreach (var studentMark in _marks)
            {
                if (studentMark.StudentId % 2 == 0)
                    _subjects.First().Marks.Add(studentMark);
                else
                    _subjects.Last().Marks.Add(studentMark);
            }
        }
    }
}