using Infrastructure.Entities;

namespace Infrastructure.Repositories.Prescription;

public interface IPrescriptionRepository
{
    Task<IEnumerable<Entities.Prescription>> GetPrescriptionsByPatientIdAsync(Guid patientId);
    
    Task<Entities.Prescription> GetPrescriptionByPatientIdAsync(Guid patientId);
    
    Task<IEnumerable<Entities.Prescription>> GetPrescriptionsByDoctorIdAsync(Guid doctorId);
    
    Task<Entities.Prescription?> GetPrescriptionByIdAsync(Guid prescriptionId);
    
    Task AddNewPrescriptionAsync(Entities.Prescription prescription);
    
    Task AddNewMedicinePrescriptionAsync(MedicinePrescription medicinePrescription);
    
}