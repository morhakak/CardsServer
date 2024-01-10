using CardsServer.Exceptions;
using CardsServer.Models;
using CardsServer.Models.UserModel;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CardsServer.Services.Users;

public class UsersService
{
    private readonly IMongoCollection<User> _users;

    public UsersService(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("dotnet_business_card_app");
        _users = database.GetCollection<User>("users");
    }

    public async Task<User> GetUserAsync(string userId)
    {
        var builder = Builders<User>.Projection;
        var projection = builder.Exclude("Password");

        var filter = Builders<User>.Filter.Eq(u => u.Id, new ObjectId(userId));

        var user = await _users.Find(filter).Project<User>(projection).FirstOrDefaultAsync();

        return user;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var builder = Builders<User>.Projection;
        var projection = builder.Exclude("Password");

        var users = await _users.Find(_ => true).Project<User>(projection).ToListAsync();

        return users;
    }

    public async Task<object> CreateUserAsync(User newUser)
    {
        var existingUser = await _users.Find(u => u.Email == newUser.Email).FirstOrDefaultAsync();
        if (existingUser != null)
        {
            throw new UserAlreadyExistsException("User with this email already exists.");
        }

        await _users.InsertOneAsync(newUser);

        return new { newUser.Id, newUser.Name, newUser.Email };
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, new ObjectId(userId));

        var result = await _users.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }

    public async Task<bool> UpdateUserAsync(string userId, User updatedUser)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, new ObjectId(userId));

        var update = Builders<User>.Update
            .Set(u => u.Name, updatedUser.Name)
            .Set(u => u.Email, updatedUser.Email)
            .Set(u => u.Address, updatedUser.Address)
            .Set(u => u.Phone, updatedUser.Phone)
            .Set(u => u.IsBussines, updatedUser.IsBussines)
            .Set(u => u.IsAdmin, updatedUser.IsAdmin)
            .Set(u => u.Image, updatedUser.Image);

        var result = await _users.UpdateOneAsync(filter, update);

        return result.MatchedCount > 0;
    }

    public async Task<User> LoginAsync(LoginModel loginModel)
    {
        var builder = Builders<User>.Projection;
        var projection = builder.Exclude("Password");

        var userLogin = await _users.Find(u => u.Email == loginModel.Email && u.Password == loginModel.Password).Project<User>(projection).FirstOrDefaultAsync();

        return userLogin ?? throw new AuthenticationException();
    }
}
