using ApplicationCore.Auth.Dto;

namespace ApplicationCore.Auth.Services;

public interface IAuthService
{
   Task<string> RegisterNewPatient(AuthDto.RegisterNewPatient request);

   Task<string> GenerateAccessToken(AuthDto.LoginDto request);
   
   Task ChangePasswordAsync(string userId, AuthDto.ChangePasswordDto request);
   
   Task<AuthDto.DetailsDto>  GetDetailsAsync(string userId);

}