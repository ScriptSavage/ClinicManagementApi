using ApplicationCore.Doctor.Dto;

namespace ApplicationCore.Doctor.Services;

public interface IDoctorService
{
    Task AddNewDoctor(DoctorDto.CreateDoctorDto dto);
}