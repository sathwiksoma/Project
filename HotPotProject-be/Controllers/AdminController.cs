using HotPotProject.Interfaces;
using HotPotProject.Models;
using HotPotProject.Models.DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotPotProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]

    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _services;

        public AdminController(IAdminServices services)
        {
            _services = services;
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDTO loginUser)
        {
            try
            {
                loginUser = await _services.LoginAdmin(loginUser);
                return Ok(loginUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(LoginUserDTO registerAdmin)
        {
            try
            {
                registerAdmin = await _services.RegisterAdmin(registerAdmin);
                return Ok(registerAdmin);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
