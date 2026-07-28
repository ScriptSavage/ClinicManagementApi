namespace Infrastructure.Repositories.Patient;

public interface IPatientRepository
{
    Task AddNewPatientAsync(Entities.Patient patient);

    Task<IEnumerable<Entities.Patient>> GetAllPatientsAsync();
    
    Task<Entities.Patient> GetPatientByIdAsync(Guid patientId);
    
    Task<Entities.Patient> GetPatientByUserIdAsync(Guid userId);
}