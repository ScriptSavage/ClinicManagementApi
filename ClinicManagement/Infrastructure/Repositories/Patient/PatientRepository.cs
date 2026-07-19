using Infrastructure.Context;

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
}