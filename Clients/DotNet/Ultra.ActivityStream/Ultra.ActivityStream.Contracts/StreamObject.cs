using System;
using System.Linq;

namespace Ultra.ActivityStream.Contracts
{
    public class Post: Ultra.ActivityStream.Contracts.StreamObject
    {
        
        public Post()
        {
            
        }
        public string PostType { get; set; }

    }
    public class StreamObject : Ultra.ActivityStream.Contracts.IStreamObject
    {

        public StreamObject()
        {

        }

        public Guid Id { get; set; }
        public string ObjectType { get; set; }
        public string DisplayName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
