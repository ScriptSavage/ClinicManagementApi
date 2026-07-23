
using ApplicationCore.Producer.Dto;

namespace ApplicationCore.Producer.Services;

public interface IProducerService
{
    Task AddNewProducer(ProducerDto.NewProducer dto);
    
    Task DeleteProducer(Guid id);
}