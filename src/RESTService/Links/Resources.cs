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
        private readonly List<Link> _links = new List<Link>();

        [DataMember]
        public IEnumerable<Link> Links => _links;

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