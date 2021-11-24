using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApp.Core.Areas.Products.Commands;
using StarterApp.Core.Areas.Products.Queries;
using StarterApp.Core.Areas.Products.ViewModels;
using StarterApp.Core.Common.Models;
using StarterApp.Core.Common.PageSort;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarterApp.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : AppControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<ProductVm>>> Get([FromQuery] PageSort<ProductVm, string> PageSort)
        {
            var result = await _mediator.Send(new GetProductListQuery(PageSort));
            return Ok(result);
        }

        [HttpGet("lookup")]
        public async Task<ActionResult<List<Lookup>>> GetLookup()
        {
            var result = await _mediator.Send(new GetProductLookupQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductVm>> GetById(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create(CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { Id = result }, new { Id = result });
        }

        [HttpPut]
        public async Task<ActionResult> Update(UpdateProductCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteProductCommand { Id = id });
            return Ok();
        }
    }
}
