using ApplicationCore.Auth.Dto;

namespace ApplicationCore.Auth.Services;

public interface IAuthService
{
   Task RegisterNewPatient(AuthDto.RegisterNewPatient request); 
}