using DataTransfer.Script;
using Server.Models;

namespace Server.Services.BusinessLogic
{
    public interface IScript
    {
        Task<ScriptDto?> Get(int mapId);
        Task<bool?> Update(ScriptDto payload);
    }

    public class Script : IScript
    {
        private readonly IRepository<DbMap> _mapRepository;

        public Script(IRepository<DbMap> mapRepository)
        {
            _mapRepository = mapRepository;
        }

        public async Task<ScriptDto?> Get(int mapId)
        {
            var map = await _mapRepository.FirstAsync(x => x.Id == mapId);

            if (map is null)
            {
                return null;
            }

            return new ScriptDto(mapId, map.Script);
        }

        public async Task<bool?> Update(ScriptDto payload)
        {
            var map = await _mapRepository.FirstAsync(x => x.Id == payload.MapId);

            if (map is null)
            {
                return null;
            }

            if (map.Script == payload.Text)
            {
                return false;
            }

            map.Script = payload.Text;

            await _mapRepository.UpdateAsync(map);

            return true;
        }
    }
}
