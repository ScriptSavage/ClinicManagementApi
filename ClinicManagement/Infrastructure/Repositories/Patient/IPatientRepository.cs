namespace Infrastructure.Repositories.Patient;

public interface IPatientRepository
{
    Task AddNewPatientAsync(Entities.Patient patient);

    Task<IEnumerable<Entities.Patient>> GetAllPatientsAsync();
    
    Task<Entities.Patient> GetPatientByIdAsync(Guid patientId);
    
    Task<Entities.Patient> GetPatientByUserIdAsync(Guid userId);
    
    void DeletePatient(Entities.Patient patient);
    
    Task<Entities.Patient?> GetPatientVisitsByIdAsync(Guid userId);
    
    Task<IEnumerable<Entities.Patient>> GetPatientPrescriptionsAsync(Guid patientId);
}