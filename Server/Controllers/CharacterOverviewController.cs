using DataTransfer.Character;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterOverviewController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;

        public CharacterOverviewController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(int campaignId)
        {
            try
            {
                var charactersInCampaign = await _dbContext.CharactersInCampaign.Where(x => x.CampaignId == campaignId).ToListAsync();

                var payload = new CharacterOverviewDto(new List<CharacterOverviewItem>());

                foreach (var characterInCampaign in charactersInCampaign)
                {
                    var character = await _dbContext.Characters.FirstAsync(x => x.Id == characterInCampaign.CharacterId);

                    var item = new CharacterOverviewItem(
                        PlayerId: characterInCampaign.UserId,
                        CharacterId: characterInCampaign.CharacterId,
                        PlayerName: "TODO", // TODO
                        CharacterName: character.Name,
                        Race: "TODO", // TODO
                        Class: "TODO", // TODO
                        Image: character.Image,
                        Level: 1, // TODO
                        Health: 20, // TODO
                        MaxHealth: 100, // TODO
                        PassivePerception: 10, // TODO
                        Strength: character.Strength,
                        Dexterity: character.Dexterity,
                        Constitution: character.Constitution,
                        Intelligence: character.Intelligence,
                        Wisdom: character.Wisdom,
                        Charisma: character.Charisma
                    );

                    payload.Items.Add(item);
                }

                return Ok(payload);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}