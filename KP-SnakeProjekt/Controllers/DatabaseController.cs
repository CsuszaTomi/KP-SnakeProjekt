using KP_SnakeProjekt.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KP_SnakeProjekt.Controllers
{
    internal class DatabaseController
    {
        private static string connectionString = "Server=127.0.0.1;Database=snakeadatb;Uid=root;Pwd=;";
        private static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
        static public Users Login(string username, string password)
        {
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            string sql = "SELECT * FROM snakeadatb.users WHERE Name=@name AND Password=@password";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@name", username);
            cmd.Parameters.AddWithValue("@password", password);
            MySqlDataReader reader = cmd.ExecuteReader();
            Users user = null;
            if (reader.Read())
            {
                int id = reader.GetInt32("ID");
                string name = reader.GetString("Name");
                string pass = reader.GetString("Password");
                reader.Close();
                user = new Users(id, name, pass);
            }
            connection.Close();
            return user;
        }
        static public bool Register(string username, string password)
        {
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            string checkSql = "SELECT COUNT(*) FROM snakeadatb.users WHERE Name=@name";
            MySqlCommand checkCmd = new MySqlCommand(checkSql, connection);
            checkCmd.Parameters.AddWithValue("@name", username);
            int count = int.Parse(checkCmd.ExecuteScalar().ToString());
            if (count > 0)
            {
                connection.Close();
                return false;
            }
            string insertSql = "INSERT INTO snakeadatb.users (Name, Password) VALUES (@name, @password)";
            MySqlCommand insertCmd = new MySqlCommand(insertSql, connection);
            insertCmd.Parameters.AddWithValue("@name", username);
            insertCmd.Parameters.AddWithValue("@password", password);
            int rows = insertCmd.ExecuteNonQuery();
            connection.Close();
            return rows > 0;
        }

        public static List<Users> GetAllUsers()
        {
            List<Users> users = new List<Users>();
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            string sql = "SELECT * FROM snakeadatb.users";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32("ID");
                string name = reader.GetString("Name");
                string pass = reader.GetString("Password");
                users.Add(new Users(id, name, pass));
            }
            connection.Close();
            return users;
        }
    }
}
