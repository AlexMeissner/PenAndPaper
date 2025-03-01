using System.Net;
using Backend.Database;
using Backend.Database.Models;
using DataTransfer.Response;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public interface IIdentity
{
    public User User { get; }

    Task<Response> Login(string email);
    Task<Response> Register(string email, string name);
}

public class Identity(PenAndPaperDatabase dbContext) : IIdentity
{
    private User? _user;

    public User User
    {
        get
        {
            if (_user is null)
            {
                throw new NullReferenceException("No user is logged in");
            }

            return _user;
        }
    }

    public async Task<Response> Login(string email)
    {
        _user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        return _user is null ? new Response(HttpStatusCode.Unauthorized) : new Response(HttpStatusCode.OK);
    }

    public async Task<Response> Register(string email, string name)
    {
        if (dbContext.Users.Any(u => u.Email == email))
        {
            return new Response(HttpStatusCode.Conflict);
        }

        var user = new User()
        {
            Email = email,
            Username = name
        };

        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return new Response(HttpStatusCode.Created);
    }
}