using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.IdentityModel.Tokens;
using Todo.Contracts;
using Todo.Contracts.Interfaces;
using Todo.Domain;
using Todo.Repository.Interfaces;

namespace Todo.Application;



 public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginUserDto authDto)
        {
            var user = await _userRepository.GetByEmailAsync(authDto.Email);
            
            if (user == null || !VerifyPasswordHash(authDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials.");

            var userDto = MapToDto(user);
            var token = GenerateJwtToken(userDto);

            return new AuthResponseDto
            {
                Token = token,
                User = userDto
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(CreateUserDto createUserDto)
        {
            if (await _userRepository.UsernameExistsAsync(createUserDto.Username))
                throw new ArgumentException("Username already exists.");

            if (await _userRepository.EmailExistsAsync(createUserDto.Email))
                throw new ArgumentException("Email already exists.");

            var user = new User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                PasswordHash = HashPassword(createUserDto.Password),
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.CreateAsync(user);
            var userDto = MapToDto(createdUser);
            var token = GenerateJwtToken(userDto);

            return new AuthResponseDto
            {
                Token = token,
                User = userDto
            };
        }

        public string GenerateJwtToken(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? ValidateJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                return userId;
            }
            catch
            {
                return null;
            }
        }

         private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }


        private bool VerifyPasswordHash(string password, string storedHash)
        {
            
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }


        private string HashPassword(string password)
        {
           
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

    }