using Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Statement.Common.DTOs;
using Statement.Query.Api.DTOs;
using Statement.Query.Api.Queries;
using Statement.Query.Domain.Entities;

namespace Statement.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StatementLookupController : ControllerBase
    {
        private readonly ILogger<StatementLookupController> _logger;
        private readonly IQueryDispatcher<StatementEntity> _queryDispatcher;

        public StatementLookupController(ILogger<StatementLookupController> logger, IQueryDispatcher<StatementEntity> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllStatementsAsync()
        {
            try
            {
                var data = await _queryDispatcher.SendAsync(new FindAllStatementsQuery());
                return NormalResponse(data);
            }
            catch (Exception ex)
            {
                const string msg = "Error while processing request to retrieve all statements";
                return ErrorResponse(ex, msg);
            }
        }

        [HttpGet("byId/{statementId}")]
        public async Task<ActionResult> GetByStatementIdAsync(Guid statementId)
        {
            try
            {
                var data = await _queryDispatcher.SendAsync(new FindStatementByIdQuery { Id = statementId });

                if (data == null || !data.Any())
                    return NoContent();

                return Ok(new StatementLookupResponse
                {
                    Statements = data,
                    Message = "Successfully returned statements"
                });
            }
            catch (Exception ex)
            {
                const string msg = "Error while processing request to find statement by ID";
                return ErrorResponse(ex, msg);
            }
        }

        [HttpGet("byAuthor/{author}")]
        public async Task<ActionResult> GetStatementsByAuthorAsync(string author)
        {
            try
            {
                var data = await _queryDispatcher.SendAsync(new FindStatementsByAuthorQuery { Author = author });
                return NormalResponse(data);
            }
            catch (Exception ex)
            {
                const string msg = "Error while processing request to find statements by author";
                return ErrorResponse(ex, msg);
            }
        }

        [HttpGet("withComments")]
        public async Task<ActionResult> GetStatementsWithCommentsAsync()
        {
            try
            {
                var data = await _queryDispatcher.SendAsync(new FindStatementsWithCommentsQuery());
                return NormalResponse(data);
            }
            catch (Exception ex)
            {
                const string msg = "Error while processing request to find statements with comments";
                return ErrorResponse(ex, msg);
            }
        }

        [HttpGet("withLikes/{numberOfLikes}")]
        public async Task<ActionResult> GetStatementsWithLikesAsync(int numberOfLikes)
        {
            try
            {
                var data = await _queryDispatcher.SendAsync(new FindStatementsWithLikesQuery { NumberOfLikes = numberOfLikes });
                return NormalResponse(data);
            }
            catch (Exception ex)
            {
                const string msg = "Error while processing request to find statements with likes";
                return ErrorResponse(ex, msg);
            }
        }

        private ActionResult NormalResponse(List<StatementEntity> data)
        {
            if (data == null || !data.Any())
                return NoContent();

            var count = data.Count;
            return Ok(new StatementLookupResponse
            {
                Statements = data,
                Message = $"Successfully returned {count} statements{(count > 1 ? "s" : string.Empty)}"
            });
        }

        private ActionResult ErrorResponse(Exception ex, string safeErrorMessage)
        {
            _logger.LogError(ex, safeErrorMessage);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = safeErrorMessage
            });
        }
    }
}