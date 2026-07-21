using ApplicationCore.Specialization.Dto;

namespace ApplicationCore.Specialization.Services;

public interface ISpecializationService
{
    Task AddNewSpecialization(SpecializationDto.AddNewSpecialization dto);
    
}