using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Specialization;


public class SpecializationRepository : ISpecializationRepository
{
    private readonly DatabaseContext _context;
    

    public SpecializationRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Entities.Specialization entity) => await _context.Specializations.AddAsync(entity);
    
    public void Delete(Entities.Specialization entity) => _context.Specializations.Remove(entity);
    
    public async Task<IEnumerable<Entities.Specialization>> GetSpecializations(IEnumerable<Guid> ids)
    {
        var data = await _context.Specializations
            .Where(e => ids.Contains(e.SpecializationId))
            .ToListAsync();
        
        return data;
    }

    public async Task<Entities.Specialization> GetSpecialization(Guid id) => await _context.Specializations
        .FirstOrDefaultAsync(e => e.SpecializationId == id);
}