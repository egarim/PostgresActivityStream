using System;
using System.Linq;

namespace Ultra.ActivityStream
{
    using System;
    public class Activity
    {
        public Guid Id { get; set; }
        public string Verb { get; set; }
        public Guid ActorId { get; set; }
        public Guid ObjectId { get; set; }
        public Guid? TargetId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Self { get; set; }
    }
}
