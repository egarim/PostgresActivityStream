using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultra.ActivityStream.Contracts
{
    public interface IFileActivityStream: IStreamObject
    {
        string Url { get; set; }
    }
    public class FileActivityStream: IFileActivityStream
    {
        
        public FileActivityStream()
        {
            
        }

        public string Url { get; set; }
        public Guid Id { get; set; }
        public string ObjectType { get; set; }
        public string DisplayName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
