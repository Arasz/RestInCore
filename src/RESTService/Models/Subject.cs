using RESTService.Providers;
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
        [DataMember]
        public IList<StudentMark> Marks { get; private set; } = new List<StudentMark>();

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public string Teacher { get; private set; }

        public Subject(string name, string teacher, IIdentityProvider<int> identityProvider) : base(identityProvider)
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
            return $"{Name}, {Teacher}, {Marks.Select(mark => $"{mark}, ")}";
        }
    }
}