using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApp.Core.Areas.Supplies.Commands;
using StarterApp.Core.Areas.Supplies.Queries;
using StarterApp.Core.Areas.Supplies.ViewModels;
using StarterApp.Core.Common.PageSort;
using System.Threading.Tasks;

namespace StarterApp.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliesController : AppControllerBase
    {
        [HttpPost("{Get}")]
        public async Task<ActionResult<PaginatedList<SupplyHeaderVm>>> Get([FromBody] PageSort<SupplyHeaderVm, SupplyHeaderFilterVm> PageSort)
        {
            var result = await _mediator.Send(new GetSupplyHeaderListQuery(PageSort));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplyHeaderVm>> GetById(int id)
        {
            var result = await _mediator.Send(new GetSupplyHeaderByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create(CreateSupplyHeaderCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { Id = result }, new { Id = result });
        }

        [HttpPut]
        public async Task<ActionResult> Update(UpdateSupplyHeaderCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteSupplyHeaderCommand { Id = id });
            return Ok();
        }
    }
}
