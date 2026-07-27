using ApplicationCore.Doctor.Dto;
using ApplicationCore.Helpers.Pagination;

namespace ApplicationCore.Doctor.Services;

public interface IDoctorService
{
    Task AddNewDoctor(DoctorDto.CreateDoctorDto dto);
    
    Task <PageResponse<DoctorDto.Response>> GetAllDoctorsAsync(int pageNumber, int pageSize);
}