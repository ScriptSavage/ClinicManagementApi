using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Patient;

public class PatientRepository : IPatientRepository
{
    private readonly DatabaseContext _context;

    public PatientRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddNewPatientAsync(Entities.Patient patient)
    {
        await _context.Patients.AddAsync(patient);
    }

    public async Task<IEnumerable<Entities.Patient>> GetAllPatientsAsync()
    {
        return await _context.Patients
            .Include(e=>e.Address)
            .AsNoTracking()
            .ToListAsync();
    }
}