using ApplicationCore.Prescription.Dto;

namespace ApplicationCore.Prescription.Service;

public interface IPrescriptionService
{
    Task AddNewPrescription(Guid patientId, string userId, PrescriptionDto.Request dto);
}