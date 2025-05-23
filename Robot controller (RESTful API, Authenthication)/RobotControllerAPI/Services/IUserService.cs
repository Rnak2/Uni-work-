using RobotControllerApi.Persistence;
using System.Threading.Tasks;

namespace RobotControllerApi.Services
{
    //interface to define user operation
    public interface IUserService
    {
        //get user by their email
        Task<UserModel?> GetUserByEmailAsync(string email);
        //for registration
        Task AddUserAsync(UserModel user); 
    }
}
