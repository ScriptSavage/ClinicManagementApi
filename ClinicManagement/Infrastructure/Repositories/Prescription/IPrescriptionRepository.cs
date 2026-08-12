namespace Infrastructure.Repositories.Prescription;

public interface IPrescriptionRepository
{
    Task<IEnumerable<Entities.Prescription>> GetPrescriptionByPatientIdAsync(Guid patientId);
    
}