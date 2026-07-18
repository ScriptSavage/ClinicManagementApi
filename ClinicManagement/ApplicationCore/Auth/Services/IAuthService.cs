using ApplicationCore.Auth.Dto;

namespace ApplicationCore.Auth.Services;

public interface IAuthService
{
   Task<string> RegisterNewPatient(AuthDto.RegisterNewPatient request);

   Task<string> GenerateAccessToken(AuthDto.LoginDto request);

}