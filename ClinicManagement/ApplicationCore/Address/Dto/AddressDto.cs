namespace ApplicationCore.Address.Dto;

public static class AddressDto
{
    public record NewAddress(
        string Street, 
        string City, 
        string ZipCode);
}