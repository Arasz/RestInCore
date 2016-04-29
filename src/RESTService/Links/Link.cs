using System.Runtime.Serialization;

namespace RESTService.Links
{
    /// <summary>
    /// A base class for relation links 
    /// </summary>
    [DataContract]
    public class Link
    {
        [DataMember]
        public string Href { get; private set; }

        [DataMember]
        public string Rel { get; private set; }

        [DataMember]
        public string Title { get; private set; }

        public Link(string relation, string href, string title = null)
        {
            Rel = relation;
            Href = href;
            Title = title;
        }
    }
}