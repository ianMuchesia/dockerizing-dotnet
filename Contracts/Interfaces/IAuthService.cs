


namespace Todo.Contracts.Interfaces;


 public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginUserDto authDto);
        Task<AuthResponseDto> RegisterAsync(CreateUserDto createUserDto);
        string GenerateJwtToken(UserDto user);
        int? ValidateJwtToken(string token);
    }