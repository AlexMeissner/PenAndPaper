using DataTransfer.Character;
using DataTransfer.WebSocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;
        private readonly IUpdateNotifier _updateNotifier;

        public CharacterController(SQLDatabase dbContext, IUpdateNotifier updateNotifier)
        {
            _dbContext = dbContext;
            _updateNotifier = updateNotifier;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int characterId)
        {
            try
            {
                var character = await _dbContext.Characters.FirstAsync(x => x.Id == characterId);
                var characterInCampaign = await _dbContext.CharactersInCampaign.FirstAsync(x => x.CharacterId == character.Id);

                var payload = new CharacterCreationDto(
                    characterInCampaign.CampaignId,
                    characterInCampaign.UserId,
                    character.Name,
                    character.Class,
                    character.Race,
                    character.Image,
                    character.Strength,
                    character.Dexterity,
                    character.Constitution,
                    character.Intelligence,
                    character.Wisdom,
                    character.Charisma);

                return Ok(payload);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CharacterCreationDto payload)
        {
            try
            {
                var character = new DbCharacter()
                {
                    Name = payload.Name,
                    Race = payload.Race,
                    Class = payload.Class,
                    ExperiencePoints = 0,
                    Strength = payload.Strength,
                    Dexterity = payload.Dexterity,
                    Constitution = payload.Constitution,
                    Intelligence = payload.Intelligence,
                    Wisdom = payload.Wisdom,
                    Charisma = payload.Charisma,
                    Image = payload.Image,
                };
                await _dbContext.Characters.AddAsync(character);
                await _dbContext.SaveChangesAsync();

                var characterInCampaign = new DbCharactersInCampaign()
                {
                    CampaignId = payload.CampaignId,
                    CharacterId = character.Id,
                    UserId = payload.UserId
                };
                await _dbContext.CharactersInCampaign.AddAsync(characterInCampaign);
                await _dbContext.SaveChangesAsync();

                await _updateNotifier.Send(payload.CampaignId, UpdateEntity.Character);

                return CreatedAtAction(nameof(Get), character.Id);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}