using RESTService.Providers;
using System;
using System.Runtime.Serialization;

namespace RESTService.Models
{
    /// <summary>
    /// Student mark data model 
    /// </summary>
    [DataContract]
    public class StudentMark : Entity
    {
        [DataMember]
        public int StudentId { get; private set; }

        [DataMember]
        public DateTime SubmitTime { get; private set; }

        [DataMember]
        public Mark Value { get; private set; }

        public StudentMark(int studentId, DateTime submitTime, Mark mark, IIdentityProvider<int> identityProvider) : base(identityProvider)
        {
            StudentId = studentId;
            SubmitTime = submitTime;
            Value = mark;
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
                return true;
            return ToString() == obj.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + StudentId.GetHashCode() + SubmitTime.GetHashCode() + Value.GetHashCode();
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