using DataTransfer;
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
        private readonly ILogger<CharacterOverviewController> _logger;

        public CharacterOverviewController(SQLDatabase dbContext, ILogger<CharacterOverviewController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<CharacterOverviewDto>>> GetAsync(int campaignId)
        {
            _logger.LogInformation(nameof(GetAsync));

            try
            {
                var charactersInCampaign = await _dbContext.CharactersInCampaign.Where(x => x.CampaignId == campaignId).ToListAsync();

                var payload = new CharacterOverviewDto()
                {
                    Items = new List<CharacterOverviewItem>()
                };

                foreach (var characterInCampaign in charactersInCampaign)
                {
                    var character = await _dbContext.Characters.FirstAsync(x => x.Id == characterInCampaign.CharacterId);

                    var item = new CharacterOverviewItem
                    {
                        PlayerId = characterInCampaign.UserId,
                        PlayerName = "TODO", // TODO
                        CharacterName = character.Name,
                        Race = "TODO", // TODO
                        Class = "TODO", // TODO
                        Image = character.Image,
                        Level = 1, // TODO
                        Health = 20, // TODO
                        MaxHealth = 100, // TODO
                        PassivePerception = 10, // TODO
                        Strength = character.Strength,
                        Dexterity = character.Dexterity,
                        Constitution = character.Constitution,
                        Intelligence = character.Intelligence,
                        Wisdom = character.Wisdom,
                        Charisma = character.Charisma,
                    };

                    payload.Items.Add(item);
                }

                var response = ApiResponse<CharacterOverviewDto>.Success(payload);

                return this.SendResponse<CharacterOverviewDto>(response);
            }
            catch (Exception exception)
            {
                var response = ApiResponse<CharacterOverviewDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
                return this.SendResponse<CharacterOverviewDto>(response);
            }
        }
    }
}