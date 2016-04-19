using System.Collections.Generic;

namespace RESTService.Models
{
    /// <summary>
    /// Subject data model 
    /// </summary>
    public class Subject
    {
        public IEnumerable<StudentMark> Marks { get; set; } = new List<StudentMark>();

        public string Name { get; set; } = "";

        public string Teacher { get; set; } = "";
    }
}