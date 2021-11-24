using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApp.Core.Areas.Categories.Commands;
using StarterApp.Core.Areas.Categories.Queries;
using StarterApp.Core.Areas.Categories.ViewModels;
using StarterApp.Core.Common.Models;
using StarterApp.Core.Common.PageSort;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarterApp.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : AppControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<CategoryVm>>> Get([FromQuery] PageSort<CategoryVm> PageSort)
        {
            var result = await _mediator.Send(new GetCategoryListQuery(PageSort));
            return Ok(result);
        }

        [HttpGet("lookup")]
        public async Task<ActionResult<List<Lookup>>> GetLookup()
        {
            var result = await _mediator.Send(new GetCategoryLookupQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryVm>> GetById(int id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create(CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { Id = result }, new { Id = result });
        }

        [HttpPut]
        public async Task<ActionResult> Update(UpdateCategoryCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteCategoryCommand { Id = id });
            return Ok();
        }
    }
}
