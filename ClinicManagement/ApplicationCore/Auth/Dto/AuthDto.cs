namespace ApplicationCore.Auth.Dto;

public static class AuthDto
{
    public record RegisterNewPatient(
        string FirstName,
        string LastName,
        string Pesel,
        DateTime DateOfBirth,
        string EmailAddress, 
        string Password,
        string ConfirmPassword,
        string PhoneNumber);
}