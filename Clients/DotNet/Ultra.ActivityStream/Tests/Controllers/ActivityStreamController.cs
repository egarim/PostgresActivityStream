using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultra.ActivityStream.AspNetCore.Api;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Ultra.ActivityStream.Contracts;
using Microsoft.AspNetCore.Http;

namespace Tests.Controllers
{
   

    [ApiController]
    [Route("[controller]")]
    public class ActivityStreamController : ActivityStreamControllerBase
    {
        public ActivityStreamController(ILogger<ActivityStreamControllerBase> logger) : base(logger)
        {

        }
        [HttpPost("CreateActivity")]
        public override Task<IActionResult> CreateActivity([FromForm(Name = "Actor")] string actor, [FromForm(Name = "Object")] string obj, [FromForm(Name = "Target")] string target, [FromForm(Name = "Latitude")] double latitude, [FromForm(Name = "Longitude")] double longitude)
        {
            return base.CreateActivity(actor, obj, target, latitude, longitude);
        }
        //[HttpPost("CreateObject")]
        //public new async Task<IActionResult> CreateObject([FromForm(Name = "Obj")] string Obj)
        //{
        //    return  Ok();
        //    //return base.CreateActivity(actor, obj, target, latitude, longitude);
        //}
        //[HttpPost("CreateObject")]
        //public override Task<IActionResult> CreateObject([FromForm(Name = "JsonObject")] string Obj, [FromForm(Name = "Files")] string Files)
        //{
        //    return base.CreateObject(Obj, Files);
        //}

        [HttpPost("FollowObjectFromIdAsync")]
        public virtual async Task<IActionResult> FollowObjectFromIdAsync([FromForm(Name = "Follower")] string Follower, [FromForm(Name = "Followee")] Guid Followee)
        {
            return Ok();
            //return base.CreateActivity(actor, obj, target, latitude, longitude);
        }

    }
}
