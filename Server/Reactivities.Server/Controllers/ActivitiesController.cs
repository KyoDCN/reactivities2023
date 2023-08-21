using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Activities;

namespace Reactivities.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new Activities.Queries.GetAll();
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new Activities.Queries.Get() { Id = id };
            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Activities.Commands.Create command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Activities.Commands.Update command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new Activities.Commands.Delete() { Id = id };
            await Mediator.Send(command);
            return Ok();
        }
    }
}
