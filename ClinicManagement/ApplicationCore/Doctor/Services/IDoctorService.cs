using ApplicationCore.Doctor.Dto;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Specialization.Dto;

namespace ApplicationCore.Doctor.Services;

public interface IDoctorService
{
    Task AddNewDoctor(DoctorDto.CreateDoctorDto dto);

    Task<PageResponse<DoctorDto.Response>> GetAllDoctorsAsync(int pageNumber, int pageSize);

    Task<DoctorDto.Response> GetDoctorByIdAsync(Guid id);

    Task UpdateDoctor(Guid id, DoctorDto.UpdateDoctorDto dto);
    
    Task DeleteDoctor(Guid id);

    Task<IEnumerable<SpecializationDto.Response>> GetDoctorSpecializationById(Guid id);
    
    Task AddNewSpecializationToDoctor(Guid id, Guid specializationId);
    
    Task DeleteDoctorSpecialization(Guid id, Guid specializationId);
    
    Task<DoctorDto.MyDetails> GetMyDetails(string userId);
}