using DataTransfer;
using DataTransfer.Map;
using Microsoft.EntityFrameworkCore;

namespace Server.Services
{
    public interface IMapOverview
    {
        public Task<ApiResponse<MapOverviewDto>> GetAsync(int campaignId);
    }

    public class MapOverview : IMapOverview
    {
        private readonly SQLDatabase _dbContext;

        public MapOverview(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<MapOverviewDto>> GetAsync(int campaignId)
        {
            MapOverviewDto payload;

            try
            {
                var maps = _dbContext.Maps.Where(x => x.CampaignId == campaignId);

                var mapOverviewItems = maps.Select(x => new MapOverviewItemDto() { Name = x.Name, MapId = x.Id, ImageData = ScaleDown(x.ImageData) });

                payload = new()
                {
                    Items = await mapOverviewItems.ToListAsync(),
                };
            }
            catch (Exception exception)
            {
                return ApiResponse<MapOverviewDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }

            return ApiResponse<MapOverviewDto>.Success(payload);
        }

        private static byte[]? ScaleDown(byte[]? originalImageData)
        {
            if (originalImageData is byte[] data)
            {
                byte[] scaledData = data;

                // TODO
                /*
            ByteArrayToBitmapImageConverter converter = new();
            BitmapImage image = new(new Uri(@"W:\PenAndPaper\LoTR\WhosThatCharacter.png"));
            var data = converter.ConvertBack(image, typeof(byte[]), image, CultureInfo.CurrentCulture) as byte[];

            MapOverview = new()
            {
                Items = new[]
                {
                    new MapOverviewItemDto() { Name="Peter", ImageData = data },
                    new MapOverviewItemDto() { Name="Hans", ImageData = data },
                    new MapOverviewItemDto() { Name="Gustav", ImageData = data },
                    new MapOverviewItemDto() { Name="Olaf", ImageData = data },
                    new MapOverviewItemDto() { Name="Günther", ImageData = data },
                }
            }; // TODO: Get from Rest API
            */

                return scaledData;
            }

            return null;
        }
    }
}