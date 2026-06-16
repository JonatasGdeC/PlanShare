using Microsoft.EntityFrameworkCore;
using PlanShare.Domain.Entities;
using PlanShare.Domain.Repositories.Association;

namespace PlanShare.Infrastructure.DataAccess.Repositories;
internal sealed class PersonAssociationRepository : IPersonAssociationReadOnlyRepository
{
    private readonly PlanShareDbContext _dbContext;

    public PersonAssociationRepository(PlanShareDbContext dbContext) => _dbContext = dbContext;

    public async Task<List<User>> GetPersonAssociationsForUser(User user)
    {
        List<PersonAssociation> associations = await _dbContext.PersonAssociations
            .AsNoTracking()
            .Include(navigationPropertyPath: association => association.AssociatedPerson)
            .Include(navigationPropertyPath: association => association.Person)
            .Where(predicate: association => association.PersonId == user.Id || association.AssociatedPersonId == user.Id)
            .ToListAsync();

        List<User> response = new List<User>();

        foreach (PersonAssociation association in associations)
        {
            if(association.PersonId == user.Id)
                response.Add(item: association.AssociatedPerson);
            else
                response.Add(item: association.Person);
        }

        return response;
    }
}
