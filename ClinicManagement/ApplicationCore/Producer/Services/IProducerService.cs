
using ApplicationCore.Medicine.Dto;
using ApplicationCore.Producer.Dto;

namespace ApplicationCore.Producer.Services;

public interface IProducerService
{
    Task AddNewProducer(ProducerDto.NewProducer dto);
    
    Task DeleteProducer(Guid id);
    
    Task<ProducerDto.NewProducer> GeProducer(Guid id);
    
    Task<IEnumerable<ProducerDto.NewProducer>> GetProducersAsync();
    
    Task<IEnumerable<MedicineDto.Response>> GetMedicinesByProducerIdAsync(Guid producerId);
    
    Task UpdateProducer(Guid producerId, ProducerDto.UpdateProducer producer);
}