using System.Net;
using Backend.Database;
using Backend.Database.Models;
using DataTransfer.Character;
using DataTransfer.Monster;
using DataTransfer.Response;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Repositories;

public interface ICharacterRepository
{
    Task<Response<int>> CreateAsync(int campaignId, int userId, CharacterCreationDto payload);
    Task<Response<CharacterDto>> GetAsync(int characterId);
    Task<Response<IEnumerable<CharactersDto>>> GetAllAsync(int campaignId);
    Task<Response<IEnumerable<CharactersDto>>> GetAllAsync(int campaignId, int userId);
}

public class CharacterRepository(PenAndPaperDatabase dbContext) : ICharacterRepository
{
    public async Task<Response<int>> CreateAsync(int campaignId, int userId, CharacterCreationDto payload)
    {
        var user = await dbContext.Users.FindAsync(userId);

        if (user is null)
        {
            return new Response<int>(HttpStatusCode.NotFound);
        }

        var character = new Character()
        {
            Name = payload.Name,
            Image = payload.Image,
            User = user,
            CampaignId = campaignId
        };

        await dbContext.AddAsync(character);
        await dbContext.SaveChangesAsync();

        return new Response<int>(HttpStatusCode.Created, character.Id);
    }

    public async Task<Response<CharacterDto>> GetAsync(int characterId)
    {
        var character = await dbContext.Characters.FindAsync(characterId);

        if (character is null)
        {
            return new Response<CharacterDto>(HttpStatusCode.NotFound);
        }

        // ToDo: Store correct values in the database and return them here
        var payload = new CharacterDto(
            character.Name,
            SizeCategory.Medium,
            "Humanoid",
            "Neutral",
            15,
            45,
            "2x D6",
            "30 ft.",
            10,
            10,
            10,
            10,
            10,
            10,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            character.Image);

        return new Response<CharacterDto>(HttpStatusCode.OK, payload);
    }

    public async Task<Response<IEnumerable<CharactersDto>>> GetAllAsync(int campaignId)
    {
        var campaign = await dbContext.Campaigns.FindAsync(campaignId);

        if (campaign == null)
        {
            return new Response<IEnumerable<CharactersDto>>(HttpStatusCode.NotFound);
        }

        var characters = dbContext.Characters
            .Where(c => c.CampaignId == campaignId)
            .Include(c => c.User)
            .Select(c => new CharactersDto(c.Id, c.Name, c.User.Username, c.Image));

        return new Response<IEnumerable<CharactersDto>>(HttpStatusCode.OK, characters);
    }

    public async Task<Response<IEnumerable<CharactersDto>>> GetAllAsync(int campaignId, int userId)
    {
        var campaign = await dbContext.Campaigns.FindAsync(campaignId);

        if (campaign == null)
        {
            return new Response<IEnumerable<CharactersDto>>(HttpStatusCode.NotFound);
        }

        var characters = dbContext.Characters
            .Where(c => c.CampaignId == campaignId && c.UserId == userId)
            .Include(c => c.User)
            .Select(c => new CharactersDto(c.Id, c.Name, c.User.Username, c.Image));

        return new Response<IEnumerable<CharactersDto>>(HttpStatusCode.OK, characters);
    }
}