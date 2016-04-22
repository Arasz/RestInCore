using FluentAssertions;
using RESTService.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace RestInCore.Tests
{
    public class StudentTests
    {
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
            var secondStudent = new Student
            {
                Id = subject.Id,
                Name = subject.Name,
                Surname = subject.Surname,
                Birthday = subject.Birthday
            };

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
            var firstStudent = new Student
            {
                Id = 1,
                Name = "T",
                Surname = "P",
                Birthday = DateTime.Parse("1992-02-12"),
            };

            bool areEqual = firstStudent.Equals(subject);

            areEqual.Should().BeFalse();
        }

        private static IEnumerable<object[]> CreateStudent(int id, string name, string surname, string birthday)
        {
            return new List<object[]>
            {
                new []{new Student() {Id = id, Name = name, Surname = surname, Birthday = DateTime.Parse(birthday)}}
            };
        }
    }
}