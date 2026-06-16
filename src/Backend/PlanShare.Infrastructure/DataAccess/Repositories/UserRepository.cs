using Microsoft.EntityFrameworkCore;
using PlanShare.Domain.Entities;
using PlanShare.Domain.Repositories.User;

namespace PlanShare.Infrastructure.DataAccess.Repositories;
internal sealed class UserRepository(PlanShareDbContext dbContext)
    : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository
{
    public async Task Add(User user) => await dbContext.Users.AddAsync(entity: user);

    public async Task<bool> ExistActiveUserWithEmail(string email) => await dbContext.Users.AnyAsync(predicate: user => user.Email.Equals(email) && user.Active);

    public async Task<User> GetById(Guid id)
    {
        return await dbContext
            .Users
            .SingleAsync(predicate: user => user.Active && user.Id == id);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await dbContext
            .Users
            .FirstOrDefaultAsync(predicate: user => user.Email.Equals(email) && user.Active);
    }

    public void Update(User user) => dbContext.Users.Update(entity: user);
}