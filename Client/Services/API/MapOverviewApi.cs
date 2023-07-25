﻿using DataTransfer.Map;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services.API
{
    public interface IMapOverviewApi
    {
        public Task<HttpResponse<MapOverviewDto>> GetAsync(int campaignId);
    }

    [TransistentService]
    public class MapOverviewApi : IMapOverviewApi
    {
        private readonly HttpRequest _request;

        public MapOverviewApi(IEndPointProvider endPointProvider)
        {
            _request = new(endPointProvider.BaseURL + "MapOverview");
        }

        public Task<HttpResponse<MapOverviewDto>> GetAsync(int campaignId)
        {
            return _request.GetAsync<MapOverviewDto>($"campaignId={campaignId}");
        }
    }
}