using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Entities.Visit?> GetVisitAsync(Guid visitId)
    {
        return await  _context.Visits.FirstOrDefaultAsync(e=>e.VisitId == visitId);
    }
}