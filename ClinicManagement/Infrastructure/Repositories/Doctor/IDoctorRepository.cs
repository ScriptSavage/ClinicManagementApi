namespace Infrastructure.Repositories.Doctor;

public interface IDoctorRepository
{
    Task AddNewDoctorAsync(Entities.Doctor doctor);
    
    Task<bool> DoesDoctorExistAsync(string Pwz);
    
    Task <IEnumerable<Entities.Doctor>>  GetAllDoctorsAsync();
    
    Task<Entities.Doctor?> GetDoctorSpecializationsByIdAsync(Guid id);
    
    Task<Entities.Doctor?> GetDoctorByIdAsync(Guid id);
    
    void DeleteDoctor(Entities.Doctor doctor);
    
    Task<IEnumerable<Entities.Doctor>> GetDoctorsBySpecialization(Entities.Specialization specialization);
    
    Task<Entities.Doctor?> FindDoctorByUserIdAsync(Guid userId);
}