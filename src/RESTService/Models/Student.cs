using RESTService.Providers;
using System;
using System.Runtime.Serialization;

namespace RESTService.Models
{
    /// <summary>
    /// Student data model 
    /// </summary>
    [DataContract]
    public class Student : Entity
    {
        [DataMember]
        public DateTime Birthday { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public string Surname { get; private set; }

        public Student(string name, string surname, DateTime birthday, IIdentityProvider<int> identityProvider) : base(identityProvider)
        {
            Name = name;
            Surname = surname;
            Birthday = birthday;
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
                return true;

            var otherStudent = obj as Student;

            return ToString() == otherStudent.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + Birthday.GetHashCode() + Name.GetHashCode() + Surname.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Id}, {Name}, {Surname}, {Birthday}";
        }
    }
}