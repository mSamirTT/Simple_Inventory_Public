using Microsoft.AspNetCore.Mvc;
using StarterApp.Core.Areas.Dashboard.Commands;
using StarterApp.Core.Areas.Dashboard.Queries;
using StarterApp.Core.Areas.Dashboard.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarterApp.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : AppControllerBase
    {
        [HttpGet("category-vs-last-week")]
        public async Task<ActionResult<List<PerformanceVsLastWeekVm>>> GetCategoryPerformaceVsLastWeek()
        {
            var result = await _mediator.Send(new CategoryVsLastWeekQuery());
            return Ok(result);
        }

        [HttpGet("least-product-vs-last-week-tile")]
        public async Task<ActionResult<PerformanceVsLastWeekVm>> GetLeastProductPerformaceVsLastWeekTile()
        {
            var result = await _mediator.Send(new LeastProductVsLastWeekQuery());
            return Ok(result);
        }

        [HttpGet("top-quantity-products")]
        public async Task<ActionResult<List<TopQtyProduct>>> GetTopQtyProducts()
        {
            var result = await _mediator.Send(new TopQuantityProductsQuery());
            return Ok(result);
        }


        [HttpGet("recent-issuings")]
        public async Task<ActionResult<List<RecentIssuingSupplyingVm>>> GetRecentIssuingsQuery()
        {
            var result = await _mediator.Send(new RecentIssuingsQuery());
            return Ok(result);
        }


        [HttpGet("recent-supplyings")]
        public async Task<ActionResult<List<RecentIssuingSupplyingVm>>> GetRecentSupplyingsQuery()
        {
            var result = await _mediator.Send(new RecentSupplyingsQuery());
            return Ok(result);
        }

        [HttpPost("log")]
        public async Task<ActionResult<List<string>>> CreateLog([FromQuery] string dateInfo)
        {
            var result = await _mediator.Send(new LogDashboardCommand
            {
                LogDateTimeZone = dateInfo
            });
            return Ok(result);
        }
    }
}
