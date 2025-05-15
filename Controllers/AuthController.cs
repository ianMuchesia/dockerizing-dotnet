using Microsoft.AspNetCore.Mvc;
using Todo.Contracts;
using Todo.Contracts.Interfaces;

namespace Todo.Controllers;



[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _authService.RegisterAsync(createUserDto);

        if (response == null)
            return BadRequest("Username or email already exists");

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto authDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _authService.LoginAsync(authDto);

        if (response == null)
            return Unauthorized("Invalid username or password");

        return Ok(response);
    }
}