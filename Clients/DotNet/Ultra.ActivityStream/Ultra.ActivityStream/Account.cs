using System;
using System.Linq;

namespace Ultra.ActivityStream
{
    using NpgsqlTypes;
    using System;
    public class Account : StreamObject, ISlug
    {

      
        public string Slug { get; set; }
    }
}
