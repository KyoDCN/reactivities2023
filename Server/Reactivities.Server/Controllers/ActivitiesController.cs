using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Activities;

namespace Reactivities.Server.Controllers
{
    public class ActivitiesController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new Activities.Queries.GetAll();
            var result = await Mediator.Send(query);

            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new Activities.Queries.Get() { Id = id };
            var result = await Mediator.Send(query);

            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Activities.Commands.Create command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Activities.Commands.Update command, Guid id)
        {
            command.Id = id;
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new Activities.Commands.Delete() { Id = id };
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id)
        {
            var command = new Activities.Commands.UpdateAttendance { Id = id };
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
    }
}
