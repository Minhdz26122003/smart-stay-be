using System;
using Npgsql;
using BCrypt.Net;

class Program
{
    static void Main()
    {
        string connStr = "Host=125.253.121.225;Port=5432;Database=smart_stay_db;Username=tofuu2202;Password=visssoft123!123";
        using var conn = new NpgsqlConnection(connStr);
        conn.Open();

        using var cmd = new NpgsqlCommand("SELECT email, password_hash FROM users LIMIT 5", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string email = reader.GetString(0);
            string hash = reader.GetString(1);
            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"Hash: {hash}");
            try 
            {
                bool isValid = BCrypt.Net.BCrypt.Verify("123456", hash);
                Console.WriteLine($"Verify '123456': {isValid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Verify Error: {ex.Message}");
            }
            Console.WriteLine("-----------------------------");
        }
    }
}
