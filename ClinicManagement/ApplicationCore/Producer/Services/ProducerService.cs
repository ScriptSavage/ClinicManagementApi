using ApplicationCore.Exceptions;
using ApplicationCore.Producer.Dto;
using FluentValidation;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Producer;

namespace ApplicationCore.Producer.Services;

public class ProducerService : IProducerService
{
    private readonly IProducerRepository _producerRepository;
    private readonly IUnitOfWork  _unitOfWork;
    private readonly IValidator<ProducerDto.NewProducer>  _validator;

    public ProducerService(IProducerRepository producerRepository,
        IUnitOfWork unitOfWork,
        IValidator<ProducerDto.NewProducer> validator)
    {
        _producerRepository = producerRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task AddNewProducer(ProducerDto.NewProducer dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var newProducer = new Infrastructure.Entities.Producer
        {
            Name = dto.Name,
        };
        await _producerRepository.CreateAsync(newProducer);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteProducer(Guid id)
    {
        var producerToDelete = await _producerRepository.GetProducer(id);
        
        if (producerToDelete is null)
        {
            throw new DoesNotExistsException();
        }
        
        _producerRepository.Delete(producerToDelete);
        
        await _unitOfWork.SaveChangesAsync();
    }
}