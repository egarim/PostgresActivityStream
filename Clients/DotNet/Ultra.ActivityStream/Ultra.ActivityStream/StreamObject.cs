using System;
using System.Linq;

namespace Brevitas.AppFramework
{
    public class StreamObject : IStreamObject
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
