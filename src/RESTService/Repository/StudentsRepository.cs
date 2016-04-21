using RESTService.Models;
using System;
using System.Collections.Generic;

namespace RESTService.Repository
{
    /// <summary>
    /// Repository containing Students 
    /// </summary>
    public class StudentsRepository : BaseRepository<Student>
    {
        public StudentsRepository()
        {
            _itemsCollection = new List<Student>
            {
                new Student(){Birthday = DateTime.Today, Name = "Andrzej", Surname = "Tkacz"},
                new Student(){Birthday = DateTime.Now, Name = "Kalina", Surname = "Wilk"}
            };
        }
    }
}