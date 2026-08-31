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

    public async Task<Entities.Visit?> GetVisitByPatientIdAsync(Guid patientId)
    {
        return await _context.Visits.FirstOrDefaultAsync(e => e.PatientId == patientId);
    }

    public async Task<IEnumerable<Entities.Visit>> GetVisitsAsync()
    {
        return _context.Visits
            .Include(e => e.Doctor)
            .Include(e => e.Patient)
            .OrderBy(e => e.Date)
            .AsNoTracking();
    }

    public async Task<IEnumerable<Entities.Visit>> GetVisitsByPatientIdAsync(Guid patientId)
    {
        return await _context.Visits
            .Include(e => e.Doctor)
            .Include(e => e.Patient)
            .OrderBy(e => e.Date)
            .Where(e => e.PatientId == patientId)
            .ToListAsync();
    }

    public async Task<Entities.Visit> GetVisitByDoctorIdAsync(Guid doctorId)
    {
        return await  _context.Visits.FirstOrDefaultAsync(e => e.DoctorId == doctorId);
    }

    public async Task<IEnumerable<Entities.Visit>> GetVisitsByDoctorIdAsync(Guid doctorId)
    {
        return await _context.Visits.Where(e => e.DoctorId == doctorId).ToListAsync();
    }

    public void DeleteVisit(Entities.Visit visit)
    {
        _context.Visits.Remove(visit);
    }
}