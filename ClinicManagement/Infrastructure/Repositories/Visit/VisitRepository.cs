using Infrastructure.Context;

namespace Infrastructure.Repositories.Visit;

public class VisitRepository : IVisitRepository
{
    private readonly DatabaseContext _context;

    public VisitRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddNewVisitAsync(Entities.Visit visit)
    {
        await _context.Visits.AddAsync(visit);
    }
}