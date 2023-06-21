using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taurus.Core.Business.CQRS.Execution.Command;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExecutionController : ControllerBase
    {
        private IMediator _mediator;
        public ExecutionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExecutionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{executionId}")]
        public async Task<IActionResult> Create([FromBody] UpdateExecutionCommand command, [FromRoute] int executionId)
        {
            command.ExecutionId = executionId;
            return Ok(await _mediator.Send(command));
        }
    }
}
