using System;
using System.Runtime.Serialization;

namespace RESTService.Models
{
    /// <summary>
    /// Student mark data model 
    /// </summary>
    [DataContract]
    public class Mark : Entity
    {
        [DataMember]
        public int StudentId { get; private set; }

        [DataMember]
        public DateTime SubmitTime { get; private set; }

        [DataMember]
        public double Value { get; private set; }

        public Mark(int studentId, DateTime submitTime, double mark)
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
            return Id.GetHashCode() + StudentId.GetHashCode() + SubmitTime.GetHashCode() + Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Id}, {StudentId}, {SubmitTime}, {Value}";
        }

        private void ValidiateMark(double value)
        {
            if (value > 5)
                Value = 5d;
            else if (value < 2)
                Value = 2d;
            else
                Value = value;
        }
    }
}