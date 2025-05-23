using Npgsql;

namespace robot_controller_api.Persistence;

public static class MapDataAccess
{
    private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=changeme;Database=sit331";

    public static List<Map> GetMaps()
    {
        var maps = new List<Map>();
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM public.map", conn);
        using var dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            var map = new Map(
                id: dr.GetInt32(0),
                name: dr.GetString(1),
                rows: dr.GetInt32(2),
                columns: dr.GetInt32(3),
                createdDate: dr.GetDateTime(4),
                modifiedDate: dr.GetDateTime(5),
                isSquare: dr.GetBoolean(6),
                description: dr.IsDBNull(7) ? null : dr.GetString(7)
            );
            maps.Add(map);
        }

        return maps;
    }

    public static Map? GetMapById(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM public.map WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("id", id);
        using var dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            return new Map(
                id: dr.GetInt32(0),
                name: dr.GetString(1),
                rows: dr.GetInt32(2),
                columns: dr.GetInt32(3),
                createdDate: dr.GetDateTime(4),
                modifiedDate: dr.GetDateTime(5),
                isSquare: dr.GetBoolean(6),
                description: dr.IsDBNull(7) ? null : dr.GetString(7)
            );
        }

        return null;
    }

    public static Map AddMap(Map newMap)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand(@"
            INSERT INTO public.map
            (name, rows, columns, createddate, modifieddate, description)
            VALUES (@name, @rows, @columns, @createddate, @modifieddate, @description)
            RETURNING *;", conn);

        cmd.Parameters.AddWithValue("name", newMap.Name);
        cmd.Parameters.AddWithValue("rows", newMap.Rows);
        cmd.Parameters.AddWithValue("columns", newMap.Columns);
        cmd.Parameters.AddWithValue("createddate", newMap.CreatedDate);
        cmd.Parameters.AddWithValue("modifieddate", newMap.ModifiedDate);
        cmd.Parameters.AddWithValue("description", (object?)newMap.Description ?? DBNull.Value);

        using var dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            return new Map(
                id: dr.GetInt32(0),
                name: dr.GetString(1),
                rows: dr.GetInt32(2),
                columns: dr.GetInt32(3),
                createdDate: dr.GetDateTime(4),
                modifiedDate: dr.GetDateTime(5),
                isSquare: dr.GetBoolean(6),
                description: dr.IsDBNull(7) ? null : dr.GetString(7)
            );
        }

        throw new Exception("Failed to insert map.");
    }

    public static bool UpdateMap(int id, Map updatedMap)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand(@"
            UPDATE public.map
            SET name = @name,
                rows = @rows,
                columns = @columns,
                modifieddate = @modifieddate,
                description = @description
            WHERE id = @id;", conn);

        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("name", updatedMap.Name);
        cmd.Parameters.AddWithValue("rows", updatedMap.Rows);
        cmd.Parameters.AddWithValue("columns", updatedMap.Columns);
        cmd.Parameters.AddWithValue("modifieddate", updatedMap.ModifiedDate);
        cmd.Parameters.AddWithValue("description", (object?)updatedMap.Description ?? DBNull.Value);

        return cmd.ExecuteNonQuery() > 0;
    }

    public static bool DeleteMap(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("DELETE FROM public.map WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("id", id);
        return cmd.ExecuteNonQuery() > 0;
    }
}
