using System;
using System.Runtime.Serialization;

namespace RESTService.Models
{
    /// <summary>
    /// Student data model 
    /// </summary>
    [DataContract]
    public class Student
    {
        [DataMember]
        public DateTime Birthday { get; set; } = DateTime.MinValue;

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; } = "";

        [DataMember]
        public string Surname { get; set; } = "";

        public override string ToString()
        {
            return $"{Id}, {Name}, {Surname}, {Birthday}";
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
                return true;

            var otherStudent = obj as Student;

            return ToString() == otherStudent.ToString();

        }
    }
}