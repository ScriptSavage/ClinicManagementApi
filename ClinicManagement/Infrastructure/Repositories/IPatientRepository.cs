using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public interface IPatientRepository
{
    Task AddNewPatientAsync(Patient patient);
}