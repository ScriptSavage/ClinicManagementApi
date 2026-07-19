namespace Infrastructure.Repositories.Doctor;

public interface IDoctorRepository
{
    Task AddNewDoctorAsync(Entities.Doctor doctor);
    
    Task<bool> DoesDoctorExistAsync(string Pwz);
}