namespace Infrastructure.Repositories.Patient;

public interface IPatientRepository
{
    Task AddNewPatientAsync(Entities.Patient patient);

    Task<IEnumerable<Entities.Patient>> GetAllPatientsAsync();
}