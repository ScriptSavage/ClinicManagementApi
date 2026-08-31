namespace Infrastructure.Repositories.Visit;

public interface IVisitRepository
{
    Task AddNewVisitAsync(Entities.Visit visit);
    
    Task<Entities.Visit?> GetVisitAsync(Guid visitId);
    
    Task<Entities.Visit?> GetVisitByPatientIdAsync(Guid patientId);
    
    Task<IEnumerable<Entities.Visit>> GetVisitsAsync();
    
    Task<IEnumerable<Entities.Visit>> GetVisitsByPatientIdAsync(Guid patientId);
    
    Task<Entities.Visit> GetVisitByDoctorIdAsync(Guid doctorId);
    
    Task<IEnumerable<Entities.Visit>> GetVisitsByDoctorIdAsync(Guid doctorId);
    
    void DeleteVisit(Entities.Visit visit);
}