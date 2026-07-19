namespace Infrastructure.Repositories.Patient;

public interface IPatientRepository
{
    Task AddNewPatientAsync(Entities.Patient patient);
}