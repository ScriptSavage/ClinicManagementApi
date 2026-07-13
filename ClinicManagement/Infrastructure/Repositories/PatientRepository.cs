using Infrastructure.Context;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly DatabaseContext _context;

    public PatientRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddNewPatientAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
    }
}