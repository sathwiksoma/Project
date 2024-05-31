using HotPotProject.Services;
using HotPotProject.Exceptions;
using HotPotProject.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HotPotProject.Context;
using HotPotProject.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationTrackerContext _context;
    private readonly AuthServices _authService;
    private readonly ITokenServices _tokenServices;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ApplicationTrackerContext context, AuthServices authService, ITokenServices tokenServices, ILogger<AuthController> logger)
    {
        _context = context;
        _authService = authService;
        _tokenServices = tokenServices;
        _logger = logger;

    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginUserDTO loginRequest)
    {
        try
        {
            // Retrieve the user from the database based on the provided email
            var user = await _authService.GetUserByUserNameAsync(loginRequest.UserName);
            if (user == null)
                throw new InvalidUserException();
            var password = getEncryptedPassword(loginRequest.Password, user.Key);
            bool matchPassword = passwordMatch(password, user.Password);
            if (matchPassword)
            {
                loginRequest.UserName = user.UserName;
                loginRequest.Password = "";
                loginRequest.Role = user.Role;
                loginRequest.Token = await _tokenServices.GenerateAllAuthorizationToken(loginRequest);
                return Ok(loginRequest);
            }

            else
            {
                // Return 401 Unauthorized if login failed
                return Unauthorized();
            }
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error occurred while logging in: {ex}");
            return StatusCode(500, "An error occurred while processing your request."); // Return 500 Internal Server Error
        }
    }
    private byte[] getEncryptedPassword(string password, byte[] key)
    {
        HMACSHA512 hmac = new HMACSHA512(key);
        var userPwd = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return userPwd;
    }
    private bool passwordMatch(byte[] password, byte[] userPassword)
    {
        for (int i = 0; i < password.Length; i++)
        {
            if (password[i] != userPassword[i])
                return false;
        }
        return true;
    }
}