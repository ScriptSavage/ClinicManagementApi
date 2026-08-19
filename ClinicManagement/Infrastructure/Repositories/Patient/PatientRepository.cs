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

    public async Task<Entities.Patient?> GetPatientByIdAsync(Guid patientId)
    {
        return await _context.Patients
            .Include(e => e.Address)
            .FirstOrDefaultAsync(e => e.PatientId == patientId);
    }

    public async Task<Entities.Patient?> GetPatientByUserIdAsync(Guid userId)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public void DeletePatient(Entities.Patient patient)
    {
        _context.Patients.Remove(patient);
    }

    public async Task<Entities.Patient?> GetPatientVisitByIdAsync(Guid userId)
    {
        return await _context.Patients
            .Include(e => e.Visits)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserId == userId);
    }

    public async Task<IEnumerable<Entities.Patient?>> GetPatientsPrescriptionsAsync(Guid patientId)
    {
        return await _context.Patients
            .Include(e => e.Prescriptions)
            .ThenInclude(e => e.MedicinePrescriptions)
            .ThenInclude(e => e.Medicine)
            .AsNoTracking()
            .Where(e => e.PatientId == patientId)
            .ToListAsync();
    }

    public async Task<Entities.Address> GetPatientAddressAsync(Guid patientId)
    {
        return await _context.Addresses.FirstOrDefaultAsync(e => e.PatientId == patientId);
    }
}