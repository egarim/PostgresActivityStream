﻿using System;
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
    public class SimpleController : ControllerBase
    {
        private readonly ILogger<SimpleController> _logger;
        public SimpleController(ILogger<SimpleController> logger)
        {
            _logger = logger;
        }
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
    }
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;

        public FilesController(ILogger<FilesController> logger)
        {
            _logger = logger;
        }

        [HttpPost()]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(
            [FromForm(Name = "Actor")] string actor,
            [FromForm(Name = "Object")] string obj,
            [FromForm(Name = "Target")] string target,
            [FromForm(Name = "Latitude")] double latitude,
            [FromForm(Name = "Longitude")] double longitude)
        {


            var test = 1;
            try
            {
                var files = new List<string>();

                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = Path.GetTempFileName();

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        files.Add(filePath);
                    }
                }

                // Do something with the files, actor, obj, target, latitude, and longitude...

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading files.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }

    //[ApiController]
    //[Route("[controller]")]
    //public class ActivityStreamController : ActivityStreamControllerBase
    //{
    //    public ActivityStreamController(ILogger<ActivityStreamControllerBase> logger) : base(logger)
    //    {

    //    }
    //    [HttpPost("upload")]
    //    public override Task<IActionResult> CreateActivity([FromForm(Name = "Actor")] StreamObject actor, [FromForm(Name = "Object")] StreamObject obj, [FromForm(Name = "Target")] StreamObject target, [FromForm(Name = "Latitude")] double latitude, [FromForm(Name = "Longitude")] double longitude)
    //    {
    //        return base.CreateActivity(actor, obj, target, latitude, longitude);
    //    }
    //}
}