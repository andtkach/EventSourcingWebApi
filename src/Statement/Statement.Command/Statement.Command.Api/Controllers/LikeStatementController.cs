using Core.Exceptions;
using Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Statement.Command.Api.Commands;
using Statement.Common.DTOs;

namespace Statement.Command.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class LikeStatementController : ControllerBase
{
    private readonly ILogger<LikeStatementController> _logger;
    private readonly ICommandDispatcher _commandDispatcher;

    public LikeStatementController(ILogger<LikeStatementController> logger, ICommandDispatcher commandDispatcher)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> LikeStatementAsync(Guid id)
    {
        try
        {
            await _commandDispatcher.SendAsync(new LikeStatementCommand { Id = id });

            return Ok(new BaseResponse
            {
                Message = "Like statement request completed successfully"
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Client made a bad request");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (AggregateNotFoundException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Could not retrieve aggregate, client passed an incorrect statement ID targetting the aggregate");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            const string msg = "Error while processing request to like a statement";
            _logger.Log(LogLevel.Error, ex, msg);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = msg
            });
        }
    }
}