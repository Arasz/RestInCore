using FluentAssertions;
using RESTService.Models;
using RESTService.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RestInCore.Tests
{
    public class StudentTests
    {
        private static IIdentityProvider<int> _identityProvider = new UniqueIdentityProvider();

        [Theory]
        [MemberData(nameof(CreateStudent), 1, "T", "P", "1992-02-12")]
        public void EqualityTest_TheSameObject_ObjectsEqual(Student subject)
        {
            var secondStudent = subject;

            bool areEqual = subject.Equals(secondStudent);

            areEqual.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(CreateStudent), 1, "T", "P", "1992-02-12")]
        public void EqualityTest_TheSameObjectsWithDifferentReferences_ObjectsEqual(Student subject)
        {
            var secondStudent = (Student)CreateStudent(1, "T", "P", "1992-02-12").First()[0];

            bool areEqual = subject.Equals(secondStudent);

            areEqual.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(CreateStudent), 13, "E", "Q", "2012-02-12")]
        [MemberData(nameof(CreateStudent), 1, "A", "P", "1992-02-12")]
        [MemberData(nameof(CreateStudent), 2, "T", "P", "1992-02-12")]
        [MemberData(nameof(CreateStudent), 1, "T", "P", "1994-04-12")]
        public void EqualityTest_TwoEqualObjectsWithDifferentReferences_ObjectsEqual(Student subject)
        {
            var firstStudent = (Student)CreateStudent(1, "T", "P", "1992-02-12").First()[0];

            bool areEqual = firstStudent.Equals(subject);

            areEqual.Should().BeFalse();
        }

        private static IEnumerable<object[]> CreateStudent(int id, string name, string surname, string birthday)
        {
            var student = new Student(name, surname, DateTime.Parse(birthday), _identityProvider);
            student.Id = id;
            return new List<object[]>
            {
                new []{student}
            };
        }
    }
}