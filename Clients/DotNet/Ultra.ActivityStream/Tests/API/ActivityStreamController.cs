using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultra.ActivityStream.AspNetCore.Api;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Ultra.ActivityStream.Contracts;

namespace Tests.API
{
    public class ActivityStreamController: ActivityStreamControllerBase
    {
        public ActivityStreamController(ILogger<ActivityStreamControllerBase> logger) : base(logger)
        {

        }
        [HttpPost("upload")]
        public override Task<IActionResult> CreateActivity([FromForm(Name = "Actor")] StreamObject actor, [FromForm(Name = "Object")] StreamObject obj, [FromForm(Name = "Target")] StreamObject target, [FromForm(Name = "Latitude")] double latitude, [FromForm(Name = "Longitude")] double longitude)
        {
            return base.CreateActivity(actor, obj, target, latitude, longitude);
        }
    }
}
