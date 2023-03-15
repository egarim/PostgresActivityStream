using System;
using System.Linq;

namespace Ultra.ActivityStream
{
    public class Account : StreamObject, ISlug
    {

      
        public string Slug { get; set; }
    }
}
