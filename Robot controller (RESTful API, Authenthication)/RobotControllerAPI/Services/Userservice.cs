using RobotControllerApi.Persistence;
using System.Threading.Tasks;

namespace RobotControllerApi.Services
{
    public class UserService : IUserService
    {
        //inject the data access layer to interact with PostgreSQL
        private readonly UserDataAccess _dataAccess;

        public UserService(UserDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        //use db access to retrieve user by email
        public Task<UserModel?> GetUserByEmailAsync(string email)
        {
            return _dataAccess.GetUserByEmailAsync(email);
        }

        //use DB access to add a new user
        public Task AddUserAsync(UserModel user)
        {
            return _dataAccess.AddUserAsync(user);
        }
    }
}
