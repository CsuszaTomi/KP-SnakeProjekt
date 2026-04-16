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
        private static string connectionString = "Server=localhost;Database=snakeadatb;Uid=root;Pwd=;";
        private static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
        /// <summary>
        /// A bejelentkezést végző metódus, amely a snakeadatb.users táblából lekéri a megadott felhasználónévhez és jelszóhoz tartozó rekordot. Ha talál ilyen rekordot, akkor létrehoz egy Users objektumot a rekord adataival, majd visszaadja azt. Ha nem talál ilyen rekordot, akkor null értéket ad vissza. Ha a lekérdezés során hiba történik, akkor egy hibaüzenetet jelenít meg és szintén null értéket ad vissza.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
        /// <summary>
        /// A regisztrációt végző metódus, amely először ellenőrzi, hogy a megadott felhasználónév már létezik-e a snakeadatb.users táblában. Ha igen, akkor false értéket ad vissza, jelezve, hogy a regisztráció sikertelen volt. Ha nem létezik, akkor beszúrja a megadott felhasználónevet és jelszót a táblába, majd true értéket ad vissza, jelezve, hogy a regisztráció sikeres volt.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Sikeres volt-e a regisztráció</returns>
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

        /// <summary>
        /// A felhasználók legmagasabb pontszámait lekérdező metódus, majd ezeket pontszám szerint csökkenő sorrendbe rendezi és visszaadja egy listában.
        /// </summary>
        /// <returns>A sorba rendezett pontok listáját</returns>
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
            // A pontszámok csökkenő sorrendbe rendezése
            scores.Sort((a, b) => b.ScorePoint.CompareTo(a.ScorePoint));
            return scores;
        }

        /// <summary>
        /// A felhasználók lekérdező metódusa, amely a snakeadatb.users táblából lekéri az összes felhasználó adatait, majd ezeket egy listában visszaadja.
        /// </summary>
        /// <returns>A felhasználók listáját</returns>
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

        /// <summary>
        /// A pontszám mentését végző metódus, amely a snakeadatb.scores táblába beszúrja a megadott felhasználó azonosítóját és a pontszámát.
        /// </summary>
        /// <param name="userId">A felhasználó id-je</param>
        /// <param name="score">A pontszám</param>
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

        /// <summary>
        /// A megadott felhasználó legmagasabb pontszámát lekérdező metódus, amely a snakeadatb.scores táblából lekéri a megadott felhasználóhoz tartozó összes pontszámot, majd ezek közül a legmagasabbat visszaadja. Ha a felhasználónak még nincs pontszáma, akkor 0-t ad vissza.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A max pontszámot</returns>
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
