﻿using DataTransfer.Map;
using System.Threading.Tasks;
using static Website.Services.ServiceExtension;

namespace Website.Services.API
{
    public interface IMapOverviewApi
    {
        public Task<HttpResponse<MapOverviewDto>> GetAsync(int campaignId);
    }

    [TransistentService]
    public class MapOverviewApi : IMapOverviewApi
    {
        private readonly HttpRequest _request;

        public MapOverviewApi(IEndPointProvider endPointProvider, ITokenProvider tokenProvider)
        {
            _request = new(endPointProvider.BaseURL + "MapOverview", tokenProvider);
        }

        public Task<HttpResponse<MapOverviewDto>> GetAsync(int campaignId)
        {
            return _request.GetAsync<MapOverviewDto>($"campaignId={campaignId}");
        }
    }
}