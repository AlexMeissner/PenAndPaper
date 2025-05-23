using DataTransfer.Monster;
using DataTransfer.Response;

namespace ApiClient;

public interface IMonsterApi
{
    Task<Response<IEnumerable<MonstersDto>>> GetAllAsync();
    Task<Response<MonsterDto>> GetAsync(int monsterId);
}

public class MonsterApi(IRequestBuilder requestBuilder) : IMonsterApi
{
    public Task<Response<IEnumerable<MonstersDto>>> GetAllAsync()
    {
        return requestBuilder.Path("dungeons-and-dragons-5e", "monsters").GetAsync<IEnumerable<MonstersDto>>();
    }

    public Task<Response<MonsterDto>> GetAsync(int monsterId)
    {
        return requestBuilder.Path("dungeons-and-dragons-5e", "monsters", monsterId).GetAsync<MonsterDto>();
    }
}