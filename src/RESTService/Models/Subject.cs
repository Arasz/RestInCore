using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace RESTService.Models
{
    /// <summary>
    /// Subject data model 
    /// </summary>
    [DataContract]
    public class Subject : Entity
    {
        public IList<Mark> Marks { get; private set; } = new List<Mark>();

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public string Teacher { get; private set; }

        public Subject(string name, string teacher)
        {
            Name = name;
            Teacher = teacher;
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
                return true;
            return ToString() == obj.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + Name.GetHashCode() + Teacher.GetHashCode() + Marks.GetHashCode();
        }

        public override string ToString()
        {
            string marksRepresentation = "";
            return $"{Name}, {Teacher}, [{Marks.Aggregate(marksRepresentation, (s, mark) => s += $"{mark}, ").TrimEnd(',')}]";
        }
    }
}