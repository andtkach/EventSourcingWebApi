using Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Statement.Command.Api.Commands;
using Statement.Command.Api.DTOs;
using Statement.Common.DTOs;

namespace Statement.Command.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewStatementController : ControllerBase
    {
        private readonly ILogger<NewStatementController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public NewStatementController(ILogger<NewStatementController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<ActionResult> NewStatementAsync(NewStatementRequest request)
        {
            var command = new NewStatementCommand()
            {
                Id = Guid.NewGuid(),
                Author = request.Author,
                Message = request.Message
            };

            try
            {
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NewStatementResponse
                {
                    Id = command.Id,
                    Message = "New statement created successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Bad request");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                const string userError = "Error while processing request to create a new statement";
                _logger.Log(LogLevel.Error, ex, userError);

                return StatusCode(StatusCodes.Status500InternalServerError, new NewStatementResponse
                {
                    Id = command.Id,
                    Message = userError
                });
            }
        }
    }
}