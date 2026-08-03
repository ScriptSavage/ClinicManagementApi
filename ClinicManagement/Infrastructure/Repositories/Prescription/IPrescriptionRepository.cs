namespace Infrastructure.Repositories.Prescription;

public interface IPrescriptionRepository
{
    Task<Entities.Prescription> GetPrescriptionByPatientIdAsync(Guid patientId);
    
    
   
}