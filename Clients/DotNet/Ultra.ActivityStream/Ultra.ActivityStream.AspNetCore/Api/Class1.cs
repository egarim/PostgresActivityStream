using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

using System.Xml.Linq;
using Ultra.ActivityStream.Contracts;

namespace Ultra.ActivityStream.AspNetCore.Api
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;

        public FilesController(ILogger<FilesController> logger)
        {
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(
            [FromForm(Name = "Actor")] StreamObject actor,
            [FromForm(Name = "Object")] StreamObject obj,
            [FromForm(Name = "Target")] StreamObject target,
            [FromForm(Name = "Latitude")] double latitude,
            [FromForm(Name = "Longitude")] double longitude)
        {
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

}