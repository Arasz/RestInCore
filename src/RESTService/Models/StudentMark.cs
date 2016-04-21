using System;
using System.Runtime.Serialization;

namespace RESTService.Models
{
    /// <summary>
    /// Student mark data model 
    /// </summary>
    [DataContract]
    public class StudentMark
    {
        [DataMember]
        public Student Student { get; set; }

        [DataMember]
        public DateTime SubmitTime { get; set; } = DateTime.Now;

        [DataMember]
        public Mark Value { get; set; } = Mark.Two;
    }

    /// <summary>
    /// Possible marks 
    /// </summary>
    public enum Mark
    {
        Two,
        TwoAndHalf,
        Three,
        ThreeAndHalf,
        Four,
        FourAndHalf,
        Five,
    }
}