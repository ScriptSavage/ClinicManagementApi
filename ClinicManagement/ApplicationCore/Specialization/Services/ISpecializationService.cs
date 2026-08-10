using ApplicationCore.Doctor.Dto;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Specialization.Dto;

namespace ApplicationCore.Specialization.Services;

public interface ISpecializationService
{
    Task AddNewSpecialization(SpecializationDto.NewSpecialization dto);
    
    Task<PageResponse<SpecializationDto.Response>>  GetSpecializations(int pageNumber, int pageSize);
    
    Task<SpecializationDto.Response> GetSpecialization(Guid specializationId);

    Task ModifySpecialization(Guid specializationId, SpecializationDto.NewSpecialization specialization);

    Task<PageResponse<DoctorDto.DoctorDetails>> GetDoctorsDetailsBySpecialization(Guid specializationId, int pageNumber,
        int pageSize); 
    
}