using System;

namespace RESTService.Models
{
    /// <summary>
    /// Student data model 
    /// </summary>
    public class Student
    {
        public DateTime Birthday { get; set; } = DateTime.MinValue;

        public string Name { get; set; } = "";

        public string Surname { get; set; } = "";

        public override string ToString()
        {
            return $"{Name}, {Surname}, {Birthday}";
        }
    }
}