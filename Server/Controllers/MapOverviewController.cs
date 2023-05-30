﻿using DataTransfer;
using DataTransfer.Map;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MapOverviewController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;

        public MapOverviewController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<MapOverviewDto>>> GetAsync(int campaignId)
        {
            try
            {
                var maps = _dbContext.Maps.Where(x => x.CampaignId == campaignId);
                var mapOverviewItems = maps.Select(x => new MapOverviewItemDto() { Name = x.Name, MapId = x.Id, ImageData = ScaleDown(x.ImageData) });

                var payload = new MapOverviewDto()
                {
                    Items = await mapOverviewItems.ToListAsync(),
                };

                var response = ApiResponse<MapOverviewDto>.Success(payload);

                return this.SendResponse<MapOverviewDto>(response);
            }
            catch (Exception exception)
            {
                var response = ApiResponse<MapOverviewDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
                return this.SendResponse<MapOverviewDto>(response);
            }
        }

        private static byte[]? ScaleDown(byte[]? originalImageData)
        {
            if (originalImageData is byte[] data)
            {
                byte[] scaledData = data;

                // TODO
                /*
                 * ByteArrayToBitmapImageConverter converter = new();
                 * BitmapImage image = new(new Uri(@"W:\PenAndPaper\LoTR\WhosThatCharacter.png"));
                 * var data = converter.ConvertBack(image, typeof(byte[]), image, CultureInfo.CurrentCulture) as byte[];
                 */

                return scaledData;
            }

            return null;
        }
    }
}