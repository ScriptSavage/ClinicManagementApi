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


    public async Task<Entities.Prescription?> GetPrescriptionByPatientIdAsync(Guid patientId)
    {
        return await _context.Prescriptions
            .FirstOrDefaultAsync(p => p.PatientId == patientId);
    }
}