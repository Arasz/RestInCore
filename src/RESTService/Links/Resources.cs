using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RESTService.Links
{
    /// <summary>
    /// Service resoruces representation 
    /// </summary>
    [DataContract]
    public class Resources
    {
        [DataMember]
        private readonly List<Link> _links = new List<Link>();

        public IEnumerable<Link> Links { get { return _links; } }

        public void AddLink(Link link)
        {
            _links.Add(link);
        }

        public void AddLinks(params Link[] links)
        {
            foreach (var link in links)
            {
                _links.Add(link);
            }
        }
    }
}