using DataTransfer;
using DataTransfer.Map;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MapOverviewController : ControllerBase
    {
        private readonly IMapOverview _mapOverview;

        public MapOverviewController(IMapOverview mapOverview)
        {
            _mapOverview = mapOverview;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<MapOverviewDto>>> GetAsync(int campaignId)
        {
            var response = await _mapOverview.GetAsync(campaignId);
            return this.SendResponse<MapOverviewDto>(response);
        }
    }
}