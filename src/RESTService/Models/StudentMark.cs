using System;

namespace RESTService.Models
{
    /// <summary>
    /// Student mark data model
    /// </summary>
    public class StudentMark
    {
        public Student Student { get; set; }

        public DateTime SubmitTime { get; set; } = DateTime.Now;

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