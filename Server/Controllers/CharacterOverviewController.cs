using DataTransfer.Character;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterOverviewController(SQLDatabase dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(int campaignId)
        {
            var campaign = await dbContext.Campaigns
                .Include(campaign => campaign.Characters)
                .ThenInclude(character => character.User)
                .FirstOrDefaultAsync(campaign => campaign.Id == campaignId);

            if (campaign is null)
            {
                return NotFound(campaignId);
            }

            var characters = campaign.Characters.Select(character => new CharacterOverviewItem(
                character.User.Id,
                character.Id,
                "TODO", // TODO
                character.Name,
                "TODO", // TODO
                "TODO", // TODO
                character.Image,
                1, // TODO
                20, // TODO
                100, // TODO
                10, // TODO
                character.Strength,
                character.Dexterity,
                character.Constitution,
                character.Intelligence,
                character.Wisdom,
                character.Charisma)).ToList();

            var payload = new CharacterOverviewDto(characters);

            return Ok(payload);
        }
    }
}
