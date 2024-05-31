using HotPotProject.Models;
using HotPotProject.Models.DTO;

namespace HotPotProject.Interfaces
{
    public interface IAdminServices
    {
        public Task<LoginUserDTO> LoginAdmin(LoginUserDTO loginUser);
        public Task<LoginUserDTO> RegisterAdmin(LoginUserDTO registerUser);
        
    }
}
