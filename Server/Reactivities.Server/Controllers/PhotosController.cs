using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Photos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Reactivities.Server.Controllers
{
    public class PhotosController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Photos.Commands.Add command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(string id)
        {
            var result = await Mediator.Send(new Photos.Commands.Remove { Id = id });
            return HandleResult(result);
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(string id)
        {
            var result = await Mediator.Send(new Photos.Commands.SetMain { Id = id });
            return HandleResult(result);
        }
    }
}
