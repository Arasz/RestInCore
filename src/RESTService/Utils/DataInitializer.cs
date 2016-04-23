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

        private readonly Dictionary<int, double> _possibleMarks = new Dictionary<int, double>
        {
            [0] = 2.0,
            [1] = 2.5,
            [2] = 3.0,
            [3] = 3.5,
            [4] = 4.0,
            [5] = 4.5,
            [6] = 5.0,
        };

        private IList<Mark> _marks = new List<Mark>();
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

            Data = _students.Concat<Entity>(_subjects).ToList();
        }

        private DateTime GenerateRandomDate(int year = 0)
        {
            if (year == 0)
                year = _random.Next(1988, 2000);

            return DateTime.Parse($"{year}-{_random.Next(1, 13)}-{_random.Next(1, 27)}");
        }

        private double GenerateRandomMark()
        {
            return _possibleMarks[_random.Next(0, 7)];
        }

        private void InitializeMarks()
        {
            foreach (var student in _students)
            {
                _marks.Add(new Mark(student.Id, GenerateRandomDate(2012), GenerateRandomMark(), _identityProvider));
                _marks.Add(new Mark(student.Id, GenerateRandomDate(2012), GenerateRandomMark(), _identityProvider));
                _marks.Add(new Mark(student.Id, GenerateRandomDate(2012), GenerateRandomMark(), _identityProvider));
            }
            _marks.OrderBy(a => _random.Next());
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
                if (_random.Next(0, 2) == 0)
                    _subjects.First().Marks.Add(studentMark);
                else
                    _subjects.Last().Marks.Add(studentMark);
            }
        }
    }
}