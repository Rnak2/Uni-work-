using Npgsql; // PostgreSQL .NET library
using RobotControllerApi.Persistence;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RobotControllerApi.Persistence
{
    // Data access methods to interact with the user table in the database
    public class UserDataAccess
    {
        // Store the database connection string configuration 
        private readonly string _connectionString = string.Empty; // Avoids nullable warning

        // Constructor that reads the PostgreSQL connection string (dependency injection)
        public UserDataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        // ✅ 1. Method to add a new user to the database
        public async Task AddUserAsync(UserModel user)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand(@"
                INSERT INTO ""user"" 
                (email, first_name, last_name, password_hash, description, role, created_date, modified_date)
                VALUES (@Email, @FirstName, @LastName, @PasswordHash, @Description, @Role, @CreatedDate, @ModifiedDate)", connection);

            // Add parameters safely to prevent SQL injection
            command.Parameters.AddWithValue("Email", user.Email);
            command.Parameters.AddWithValue("FirstName", user.FirstName);
            command.Parameters.AddWithValue("LastName", user.LastName);
            command.Parameters.AddWithValue("PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("Description", (object?)user.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("Role", (object?)user.Role ?? DBNull.Value);
            command.Parameters.AddWithValue("CreatedDate", user.CreatedDate);
            command.Parameters.AddWithValue("ModifiedDate", user.ModifiedDate);

            await command.ExecuteNonQueryAsync();
        }

        // ✅ 2. Method to retrieve a single user by email
        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand(@"SELECT * FROM ""user"" WHERE email = @Email", connection);
            command.Parameters.AddWithValue("Email", email);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapUser(reader);
            }

            return null; // User not found
        }

        // ✅ 3. Method to retrieve a single user by ID
        public async Task<UserModel?> GetUserByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand(@"SELECT * FROM ""user"" WHERE id = @Id", connection);
            command.Parameters.AddWithValue("Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapUser(reader);
            }

            return null; // User not found
        }

        // ✅ 4. Method to get all users (Admin Only)
        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            var users = new List<UserModel>();

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new NpgsqlCommand("SELECT * FROM \"user\"", connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(MapUser(reader));
            }

            return users;
        }

        // ✅ 5. Method to update an existing user
        public async Task UpdateUserAsync(UserModel user)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand(@"
                UPDATE ""user"" 
                SET first_name = @FirstName, 
                    last_name = @LastName, 
                    password_hash = @PasswordHash, 
                    description = @Description, 
                    role = @Role, 
                    modified_date = @ModifiedDate 
                WHERE id = @Id", connection);

            // Add parameters safely
            command.Parameters.AddWithValue("FirstName", user.FirstName);
            command.Parameters.AddWithValue("LastName", user.LastName);
            command.Parameters.AddWithValue("PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("Description", (object?)user.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("Role", (object?)user.Role ?? DBNull.Value);
            command.Parameters.AddWithValue("ModifiedDate", DateTime.UtcNow);
            command.Parameters.AddWithValue("Id", user.Id);

            await command.ExecuteNonQueryAsync();
        }

        // ✅ 6. Method to delete a user by ID
        public async Task DeleteUserByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand(@"DELETE FROM ""user"" WHERE id = @Id", connection);
            command.Parameters.AddWithValue("Id", id);

            await command.ExecuteNonQueryAsync();
        }

        // Method to delete a user by Email
        public async Task DeleteUserByEmailAsync(string email)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand(@"DELETE FROM ""user"" WHERE email = @Email", connection);
            command.Parameters.AddWithValue("Email", email);

            await command.ExecuteNonQueryAsync();
        }

        // Private Helper: Map database row to UserModel
        private UserModel MapUser(NpgsqlDataReader reader)
        {
            return new UserModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Email = reader.GetString(reader.GetOrdinal("email")),
                FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                LastName = reader.GetString(reader.GetOrdinal("last_name")),
                PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                Role = reader.IsDBNull(reader.GetOrdinal("role")) ? null : reader.GetString(reader.GetOrdinal("role")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("created_date")),
                ModifiedDate = reader.GetDateTime(reader.GetOrdinal("modified_date"))
            };
        }
    }
}
