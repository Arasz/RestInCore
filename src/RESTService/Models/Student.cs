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
        public string Name { get; set; } = "";

        [DataMember]
        public string Surname { get; set; } = "";

        public override string ToString()
        {
            return $"{Name}, {Surname}, {Birthday}";
        }
    }
}