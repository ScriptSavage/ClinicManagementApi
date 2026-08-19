using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Prescription;

public class PrescriptionRepository :  IPrescriptionRepository
{
    private readonly DatabaseContext _context;

    public PrescriptionRepository(DatabaseContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Entities.Prescription>> GetPrescriptionsByPatientIdAsync(Guid patientId)
    {
        return await _context.Prescriptions
            .Include(e=>e.Doctor)
            .Include(t=>t.MedicinePrescriptions)
            .ThenInclude(p=>p.Medicine)
            .Where(e => e.PatientId == patientId)
            .ToListAsync();
    }

    public async Task<Entities.Prescription?> GetPrescriptionByPatientIdAsync(Guid patientId)
    {
        return await _context.Prescriptions
            .FirstOrDefaultAsync(e => e.PatientId == patientId);
    }

    public async Task<IEnumerable<Entities.Prescription>> GetPrescriptionsByDoctorIdAsync(Guid doctorId)
    {
        return await _context.Prescriptions
            .Where(e => e.DoctorId == doctorId)
            .Include(p=>p.Patient)
            .Include(e => e.MedicinePrescriptions)
            .ThenInclude(p => p.Medicine)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }
}