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
        public int StudentId { get; set; }

        [DataMember]
        public DateTime SubmitTime { get; set; } = DateTime.Now;

        [DataMember]
        public Mark Value { get; set; } = Mark.Two;

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
                return true;
            return ToString() == obj.ToString();
        }

        public override string ToString()
        {
            return $"{StudentId}, {SubmitTime}, {Value}";
        }
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