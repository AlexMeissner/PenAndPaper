using DataTransfer.Monster;
using System.Threading.Tasks;
using static Website.Services.ServiceExtension;

namespace Website.Services.API
{
    public interface IMonsterApi
    {
        public Task<HttpResponse<MonsterDto>> Get(int monsterId);
        public Task<HttpResponse<MonstersDto>> GetAll();
    }

    [TransistentService]
    public class MonsterApi(IEndPointProvider endPointProvider, ITokenProvider tokenProvider) : IMonsterApi
    {
        private readonly HttpRequest _request = new(endPointProvider.BaseURL + "Monster", tokenProvider);

        public Task<HttpResponse<MonsterDto>> Get(int monsterId)
        {
            return _request.GetAsync<MonsterDto>($"monsterId={monsterId}");
        }

        public Task<HttpResponse<MonstersDto>> GetAll()
        {
            return _request.GetAsync<MonstersDto>();
        }
    }
}
