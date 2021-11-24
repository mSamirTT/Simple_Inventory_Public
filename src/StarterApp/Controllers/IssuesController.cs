using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApp.Core.Areas.Issues.Commands;
using StarterApp.Core.Areas.Issues.Queries;
using StarterApp.Core.Areas.Issues.ViewModels;
using StarterApp.Core.Common.PageSort;
using System.Threading.Tasks;

namespace StarterApp.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : AppControllerBase
    {
        [HttpPost("{Get}")]
        public async Task<ActionResult<PaginatedList<IssueHeaderVm>>> Get([FromBody] PageSort<IssueHeaderVm, IssueHeaderFilterVm> PageSort)
        {
            var result = await _mediator.Send(new GetIssueHeaderListQuery(PageSort));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IssueHeaderVm>> GetById(int id)
        {
            var result = await _mediator.Send(new GetIssueHeaderByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create(CreateIssueHeaderCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { Id = result }, new { Id = result });
        }

        [HttpPut]
        public async Task<ActionResult> Update(UpdateIssueHeaderCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteIssueHeaderCommand { Id = id });
            return Ok();
        }
    }
}
