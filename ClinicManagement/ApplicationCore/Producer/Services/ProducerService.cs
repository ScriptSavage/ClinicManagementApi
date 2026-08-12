using ApplicationCore.Exceptions;
using ApplicationCore.Medicine.Dto;
using ApplicationCore.Producer.Dto;
using FluentValidation;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Medicine;
using Infrastructure.Repositories.Producer;

namespace ApplicationCore.Producer.Services;

public class ProducerService : IProducerService
{
    private readonly IProducerRepository _producerRepository;
    private readonly IMedicineRepository _medicineRepository;
    private readonly IUnitOfWork  _unitOfWork;
    private readonly IValidator<ProducerDto.NewProducer>  _validator;

    public ProducerService(IProducerRepository producerRepository,
        IMedicineRepository medicineRepository,
        IUnitOfWork unitOfWork,
        IValidator<ProducerDto.NewProducer> validator)
    {
        _producerRepository = producerRepository;
        _medicineRepository = medicineRepository;
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

    public async Task<ProducerDto.NewProducer> GeProducer(Guid id)
    {
        var producerToGet = await _producerRepository.GetProducer(id);
        
        if (producerToGet is null)
        {
            throw new DoesNotExistsException("Producer not found");
        }
        
        return new ProducerDto.NewProducer(producerToGet.Name);
    }

    public async Task<IEnumerable<ProducerDto.NewProducer>> GetProducersAsync()
    {
        var producers = await _producerRepository.GetProducersAsync();
        
        return producers.Select(producer => new ProducerDto.NewProducer(producer.Name));
    }

    public async Task<IEnumerable<MedicineDto.Response>> GetMedicinesByProducerIdAsync(Guid producerId)
    {
        var producer = await _producerRepository.GetProducer(producerId);

        if (producer is null)
        {
            throw new DoesNotExistsException("Producer not found");
        }

        var medicines = await _medicineRepository
            .GetMedicinesByProducerIdAsync(producerId);

        return medicines.Select(e => new MedicineDto.Response(
            e.Name,
            e.ActiveSubstance,
            e.PharmaceuticalForm));
    }
}