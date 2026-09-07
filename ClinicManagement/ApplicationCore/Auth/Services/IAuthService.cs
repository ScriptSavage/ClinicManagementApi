using ApplicationCore.Auth.Dto;

namespace ApplicationCore.Auth.Services;

public interface IAuthService
{
   Task<string> RegisterNewPatient(AuthDto.RegisterNewPatient request);

   Task<AuthDto.AuthResponse> LoginAsync(AuthDto.LoginDto request);

   Task<AuthDto.AuthResponse> RefreshAsync(AuthDto.RefreshTokenDto request);

   Task LogoutAsync(AuthDto.RefreshTokenDto request);
   
   Task ChangePasswordAsync(string userId, AuthDto.ChangePasswordDto request);
   
   Task<AuthDto.DetailsDto>  GetDetailsAsync(string userId);

}
