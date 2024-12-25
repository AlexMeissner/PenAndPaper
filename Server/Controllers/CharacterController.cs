using DataTransfer.Character;
using DataTransfer.WebSocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController(SQLDatabase dbContext, IUpdateNotifier updateNotifier) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(int? characterId, int? userId, int? campaignId)
        {
            if (characterId is not null)
            {
                var character = await dbContext.Characters
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == characterId);

                if (character is null)
                {
                    return NotFound(characterId);
                }

                var payload = new CharacterCreationDto(
                    character.CampaignId,
                    character.User.Id,
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
            else if (userId is not null && campaignId is not null)
            {
                var character = await dbContext.Characters
                .Include(c => c.User)
                .Include(c => c.Campaign)
                .FirstOrDefaultAsync(c => c.Campaign.Id == campaignId && c.User.Id == userId);

                if (character is null)
                {
                    return NotFound();
                }

                var payload = new CharacterCreationDto(
                    character.CampaignId,
                    character.User.Id,
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

            return BadRequest("Invalid parameters.");
        }

        [HttpPost]
        public async Task<IActionResult> Post(CharacterCreationDto payload)
        {
            var user = await dbContext.Users.FindAsync(payload.UserId);

            if (user is null)
            {
                return NotFound(payload.UserId);
            }

            var character = new Character()
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
                User = user,
                CampaignId = payload.CampaignId
            };
            await dbContext.AddAsync(character);
            await dbContext.SaveChangesAsync();

            await updateNotifier.Send(payload.CampaignId, UpdateEntity.Character);

            return CreatedAtAction(nameof(Get), character.Id);
        }
    }
}