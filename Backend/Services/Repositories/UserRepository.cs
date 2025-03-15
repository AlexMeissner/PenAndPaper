using System.Net;
using Backend.Database;
using DataTransfer.Campaign;
using DataTransfer.Response;

namespace Backend.Services.Repositories;

public interface IUserRepository
{
    Response<IEnumerable<CampaignUser>> GetAll(IdentityClaims identity);
}

public class UserRepository(PenAndPaperDatabase dbContext) : IUserRepository
{
    public Response<IEnumerable<CampaignUser>> GetAll(IdentityClaims identity)
    {
        var users = dbContext.Users
            .Where(u => u != identity.User)
            .Select(u => new CampaignUser(u.Id, u.Username));

        return new Response<IEnumerable<CampaignUser>>(HttpStatusCode.OK, users);
    }
}