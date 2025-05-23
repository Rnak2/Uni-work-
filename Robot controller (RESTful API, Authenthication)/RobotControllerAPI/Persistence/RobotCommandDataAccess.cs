using Npgsql;

namespace robot_controller_api.Persistence;

public static class RobotCommandDataAccess
{
    private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=changeme;Database=sit331";

    // Get all robot commands
    public static List<RobotCommand> GetRobotCommands()
    {
        var robotCommands = new List<RobotCommand>();
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM public.robotcommand", conn);
        using var dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            var command = new RobotCommand(
                id: dr.GetInt32(0),
                name: dr.GetString(1),
                isMoveCommand: dr.GetBoolean(3),
                createdDate: dr.GetDateTime(4),
                modifiedDate: dr.GetDateTime(5),
                description: dr.IsDBNull(2) ? null : dr.GetString(2)
            );
            robotCommands.Add(command);
        }

        return robotCommands;
    }

    // Get a command by ID
    public static RobotCommand? GetRobotCommandById(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM public.robotcommand WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("id", id);
        using var dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            return new RobotCommand(
                id: dr.GetInt32(0),
                name: dr.GetString(1),
                isMoveCommand: dr.GetBoolean(3),
                createdDate: dr.GetDateTime(4),
                modifiedDate: dr.GetDateTime(5),
                description: dr.IsDBNull(2) ? null : dr.GetString(2)
            );
        }

        return null;
    }

    // Add a new robot command
    public static RobotCommand AddRobotCommand(RobotCommand newCommand)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand(@"
            INSERT INTO public.robotcommand 
            (name, description, ismovecommand, createddate, modifieddate) 
            VALUES (@name, @description, @ismovecommand, @createddate, @modifieddate)
            RETURNING id, name, description, ismovecommand, createddate, modifieddate;", conn);

        cmd.Parameters.AddWithValue("name", newCommand.Name);
        cmd.Parameters.AddWithValue("description", (object?)newCommand.Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("ismovecommand", newCommand.IsMoveCommand);
        cmd.Parameters.AddWithValue("createddate", newCommand.CreatedDate);
        cmd.Parameters.AddWithValue("modifieddate", newCommand.ModifiedDate);

        using var dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            return new RobotCommand(
                id: dr.GetInt32(0),
                name: dr.GetString(1),
                isMoveCommand: dr.GetBoolean(3),
                createdDate: dr.GetDateTime(4),
                modifiedDate: dr.GetDateTime(5),
                description: dr.IsDBNull(2) ? null : dr.GetString(2)
            );
        }

        throw new Exception("Failed to insert RobotCommand.");
    }

    // Update an existing command
    public static bool UpdateRobotCommand(int id, RobotCommand updatedCommand)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand(@"
            UPDATE public.robotcommand
            SET name = @name,
                description = @description,
                ismovecommand = @ismovecommand,
                modifieddate = @modifieddate
            WHERE id = @id;", conn);

        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("name", updatedCommand.Name);
        cmd.Parameters.AddWithValue("description", (object?)updatedCommand.Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("ismovecommand", updatedCommand.IsMoveCommand);
        cmd.Parameters.AddWithValue("modifieddate", updatedCommand.ModifiedDate);

        int rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected > 0;
    }

    // Delete a command by ID
    public static bool DeleteRobotCommand(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("DELETE FROM public.robotcommand WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("id", id);
        int rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected > 0;
    }
}
