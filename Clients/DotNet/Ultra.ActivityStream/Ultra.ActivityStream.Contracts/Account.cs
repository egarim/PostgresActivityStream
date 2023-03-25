using System;
using System.Linq;

namespace Ultra.ActivityStream.Contracts
{
    
    public class Account : Ultra.ActivityStream.Contracts.StreamObject, Ultra.ActivityStream.Contracts.ISlug
    {

      
        public string Slug { get; set; }
    }
}
