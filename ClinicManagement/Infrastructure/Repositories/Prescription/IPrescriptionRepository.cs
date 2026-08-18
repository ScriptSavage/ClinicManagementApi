namespace Infrastructure.Repositories.Prescription;

public interface IPrescriptionRepository
{
    Task<IEnumerable<Entities.Prescription>> GetPrescriptionsByPatientIdAsync(Guid patientId);
    
    Task<IEnumerable<Entities.Prescription>> GetPrescriptionsByDoctorIdAsync(Guid doctorId);
    
    
}