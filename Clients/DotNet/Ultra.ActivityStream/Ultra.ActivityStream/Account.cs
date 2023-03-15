using System;
using System.Linq;

namespace Brevitas.AppFramework
{
    public class Account : StreamObject, ISlug
    {

      
        public string Slug { get; set; }
    }
}
