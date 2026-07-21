using ApplicationCore.Specialization.Dto;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Specialization;

namespace ApplicationCore.Specialization.Services;

public class SpecializationService : ISpecializationService
{
    private readonly ISpecializationRepository _specializationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SpecializationService(ISpecializationRepository specializationRepository, 
        IUnitOfWork unitOfWork)
    {
        _specializationRepository = specializationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task AddNewSpecialization(SpecializationDto.AddNewSpecialization dto)
    {
        var newSpecialization = new Infrastructure.Entities.Specialization
        {
            Name = dto.SpecializationName,
            Description = dto.SpecializationDescription,
        };

        await _specializationRepository.CreateAsync(newSpecialization);
        await _unitOfWork.SaveChangesAsync();
    }
}