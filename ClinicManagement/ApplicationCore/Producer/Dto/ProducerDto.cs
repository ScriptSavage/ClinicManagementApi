namespace ApplicationCore.Producer.Dto;

public static class ProducerDto
{
    public record NewProducer(
        string Name);

    public record UpdateProducer(string Name) : NewProducer(Name);
}