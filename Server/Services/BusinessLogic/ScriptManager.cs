using DataTransfer.Script;
using Server.Models;

namespace Server.Services.BusinessLogic
{
    public interface IScriptManager
    {
        Task<ScriptDto?> Get(int mapId);
        Task<bool?> Update(ScriptDto payload);
    }

    public class ScriptManager(IDatabaseContext dbContext, IRepository<Map> mapRepository) : IScriptManager
    {
        public async Task<ScriptDto?> Get(int mapId)
        {
            var map = await mapRepository.FindAsync(mapId);

            if (map is null)
            {
                return null;
            }

            return new ScriptDto(mapId, map.Script);
        }

        public async Task<bool?> Update(ScriptDto payload)
        {
            var map = await mapRepository.FindAsync(payload.MapId);

            if (map is null)
            {
                return null;
            }

            if (map.Script == payload.Text)
            {
                return false;
            }

            map.Script = payload.Text;

            await dbContext.UpdateAsync(map);

            return true;
        }
    }
}
