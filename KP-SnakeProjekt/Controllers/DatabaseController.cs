using KP_SnakeProjekt.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Hiba a bejelentkezéskor: " + ex.Message);
                return null;
            }
        }
        static public bool Register(string username, string password)
        {
            try
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
            catch(Exception ex)
            {
                MessageBox.Show("Hiba a felhasználó regisztrálásakor: " + ex.Message);
                return false;
            }
        }

        public static List<Score> GetTopScores()
        {
            List<Users> users = GetAllUsers();
            List<Score> scores = new List<Score>();
            foreach (Users user in users)
            {
                int maxScore = GetMaxScore(user.Id);
                Score score = new Score();
                score.Name = user.UserName;
                score.ScorePoint = maxScore;
                scores.Add(score);
            }
            scores.Sort((a, b) => b.ScorePoint.CompareTo(a.ScorePoint));
            return scores;
        }

        public static List<Users> GetAllUsers()
        {
            try
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
            catch(Exception ex)
            {
                MessageBox.Show("Hiba a felhasználók lekérésekor: " + ex.Message);
                return new List<Users>();
            }
        }

        public static void AddScore(int userId, int score)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                string sql = "INSERT INTO snakeadatb.scores (UserID, Score) VALUES (@userId, @score)";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@score", score);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Hiba a pontszám mentésekor: " + ex.Message);
            }
        }

        public static int GetMaxScore(int userId)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                string sql = "SELECT MAX(Score) FROM snakeadatb.scores WHERE UserID=@userId";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@userId", userId);
                object result = cmd.ExecuteScalar();
                connection.Close();
                if (result == null || result == DBNull.Value)
                {
                    return 0;
                }
                return int.Parse(result.ToString());
            }
            catch(Exception ex)
            {     
                MessageBox.Show("Hiba a pontszám lekérésekor: " + ex.Message);
                return 0;
            }
        }
    }
}
