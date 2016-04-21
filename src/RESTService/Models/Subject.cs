using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RESTService.Models
{
    /// <summary>
    /// Subject data model 
    /// </summary>
    [DataContract]
    public class Subject
    {
        [DataMember]
        public IEnumerable<StudentMark> Marks { get; set; } = new List<StudentMark>();

        [DataMember]
        public string Name { get; set; } = "";

        [DataMember]
        public string Teacher { get; set; } = "";
    }
}