using ApplicationCore.Address.Dto;

using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Auth.Dto;

public static class AuthDto
{

    public record AuthResponse(string Token, string RefreshToken);

    public record RefreshTokenDto([Required, StringLength(88, MinimumLength = 88)] string RefreshToken);

    public record RegisterNewPatient(
        string FirstName,
        string LastName,
        string Pesel,
        DateTime DateOfBirth,
        string EmailAddress, 
        string Password,
        string ConfirmPassword,
        string PhoneNumber,
        AddressDto.NewAddress Address);

    public record LoginDto(
        string Username,
        string Password);


    public record ChangePasswordDto(
        string CurrentPassword,
        string NewPassword,
        string ConfirmPassword
    );

    public record DetailsDto(
        string FirstName,
        string LastName,
        string Pesel,
        DateTime DateOfBirth,
        string EmailAddress,
        string PhoneNumber,
        AddressDto.NewAddress Address
    );

    public record UserDetailsDto(
        string Username,
        string Email,
        string? FirstName,
        string? LastName,
        string RoleName
    );

}
