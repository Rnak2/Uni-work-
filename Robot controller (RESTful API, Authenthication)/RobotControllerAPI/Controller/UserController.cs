using Microsoft.AspNetCore.Mvc; //for ControllerBase, ApiController, HttpPost, etc.
using RobotControllerApi.Persistence; //for UserDataAccess
using Microsoft.AspNetCore.Identity; //for PasswordHasher
using Microsoft.AspNetCore.Authorization;

namespace RobotControllerApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly UserDataAccess _dataAccess;
        private readonly PasswordHasher<UserModel> _hasher = new(); //handles secure password hashing

        //constructor with dependency injection for database access
        public UsersController(UserDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        /// <summary>
        /// Register a new user (hash password and save to DB)
        /// </summary>
        [HttpPost]
public async Task<ActionResult<UserModel>> Register(UserModel user)
{
    // Check if email already exists
    var existingUser = await _dataAccess.GetUserByEmailAsync(user.Email);
    if (existingUser != null)
    {
        return Conflict(new { message = "User with this email already exists." });
    }

    // Hash the password securely
    user.PasswordHash = _hasher.HashPassword(user, user.PasswordHash);
    user.CreatedDate = DateTime.UtcNow;
    user.ModifiedDate = DateTime.UtcNow;

    //  Store the user in the database
    await _dataAccess.AddUserAsync(user);

    //Return user details without the password
    return Ok(new 
    {
        user.Id,
        user.Email,
        user.FirstName,
        user.LastName,
        user.Role,
        user.CreatedDate,
        user.ModifiedDate
    });
}

        /// <summary>
        /// Login with email and password
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginModel login)
        {
            // retrieve user from the database
            var user = await _dataAccess.GetUserByEmailAsync(login.Email);
            if (user == null)
                return Unauthorized("User not found");

            // verify the password using hashed value
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid password");

            return Ok("Login successful");
        }

        /// <summary>
        /// Retrieve all users (admin only in production)
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAll()
        {
            var users = await _dataAccess.GetAllUsersAsync(); //get user from db
            return Ok(users); //return list of users
        }

        /// <summary>
/// Update user details (Admin Only)
/// </summary>
[Authorize(Policy = "AdminOnly")]
[HttpPut("email/{email}")] // Use email in the route
public async Task<IActionResult> UpdateUserByEmail(string email, [FromBody] UserModel updatedUser)
{
    var user = await _dataAccess.GetUserByEmailAsync(email); // Search by email
    if (user == null)
        return NotFound("User not found.");

    user.FirstName = updatedUser.FirstName;
    user.LastName = updatedUser.LastName;
    user.Role = updatedUser.Role ?? user.Role;
    user.Description = updatedUser.Description ?? user.Description;

    //Hash password only if it is updated
    if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
    {
        user.PasswordHash = _hasher.HashPassword(user, updatedUser.PasswordHash);
    }

    user.ModifiedDate = DateTime.UtcNow;
    await _dataAccess.UpdateUserAsync(user);

    return NoContent();
}


        /// <summary>
        /// Delete user by ID (Admin Only)
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _dataAccess.DeleteUserByIdAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Delete user by Email (Admin Only)
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("email/{email}")]
        public async Task<IActionResult> DeleteUserByEmail(string email)
        {
            await _dataAccess.DeleteUserByEmailAsync(email);
            return NoContent();
        }
    }
}
