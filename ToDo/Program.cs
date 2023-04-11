using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;

namespace TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.CheckDatabase();

            Menu.DisplayMenu();
        }
    }

    public static class Menu
    {
        public static void DisplayMenu()
        {
            Console.WriteLine("Welcome to Task Manager!");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    Register();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input.");
                    DisplayMenu();
                    break;
            }
        }

        private static void Login()
        {
            Console.WriteLine("Enter your username:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            string password = Console.ReadLine();

            if (Database.Login(username, password))
            {
                Console.WriteLine("Login successful.");
                UserMenu(username);
            }
            else
            {
                Console.WriteLine("Invalid username or password.");
                DisplayMenu();
            }
        }

        private static void Register()
        {
            Console.WriteLine("Enter a username:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter a password:");
            string password = Console.ReadLine();

            if (Database.Register(username, password))
            {
                Console.WriteLine("Registration successful.");
                UserMenu(username);
            }
            else
            {
                Console.WriteLine("Registration failed.");
                DisplayMenu();
            }
        }

        private static void UserMenu(string username)
        {
            Console.WriteLine($"Welcome {username}!");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Create new task");
            Console.WriteLine("2. View tasks");
            Console.WriteLine("3. Edit task");
            Console.WriteLine("4. Delete task");
            Console.WriteLine("5. Logout");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    CreateTask(username);
                    break;
                case "2":
                    ViewTasks(username);
                    break;
                case "3":
                    EditTask(username);
                    break;
                case "4":
                    DeleteTask(username);
                    break;
                case "5":
                    Logout();
                    break;
                default:
                    Console.WriteLine("Invalid input.");
                    UserMenu(username);
                    break;
            }
        }

        private static void CreateTask(string username)
        {
            Console.WriteLine("Enter task description:");
            string description = Console.ReadLine();

            if (Database.CreateTask(username, description))
            {
                Console.WriteLine("Task created successfully.");
            }
            else
            {
                Console.WriteLine("Task creation failed.");
            }

            UserMenu(username);
        }

        private static void ViewTasks(string username)
        {
            List<Task> tasks = Database.GetTasks(username);

            if (tasks.Count > 0)
            {
                foreach (Task task in tasks)
                {
                    Console.WriteLine($"{task.Id}: {task.Description}");
                }
            }
            else
            {
                Console.WriteLine("No tasks found.");
            }

            UserMenu(username);
        }

        private static void EditTask(string username)
        {
            Console.WriteLine("Enter the ID of the task you want to edit:");
            string input = Console.ReadLine();
            int taskId;
            if (int.TryParse(input, out taskId))
            {
                Task task = Database.GetTask(taskId);
                if (task != null && task.Username == username)
                {
                    Console.WriteLine("Enter new task description:");
                    string description = Console.ReadLine();

                    if (Database.EditTask(taskId, description))
                    {
                        Console.WriteLine("Task edited successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Task edit failed.");
                    }
                }
                else
                {
                    Console.WriteLine("Task not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }

            UserMenu(username);
        }

        private static void DeleteTask(string username)
        {
            Console.WriteLine("Enter the ID of the task you want to delete:");
            string input = Console.ReadLine();
            int taskId;
            if (int.TryParse(input, out taskId))
            {
                Task task = Database.GetTask(taskId);
                if (task != null && task.Username == username)
                {
                    if (Database.DeleteTask(taskId))
                    {
                        Console.WriteLine("Task deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Task deletion failed.");
                    }
                }
                else
                {
                    Console.WriteLine("Task not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }

            UserMenu(username);
        }

        private static void Logout()
        {
            DisplayMenu();
        }
    }

    public class Task
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Description { get; set; }
    }

    public static class Database
    {
        private static string connectionString = "Data Source=TaskManager.db";

        public static void CheckDatabase()
        {
            // Check if database exists and create if necessary
            if (!System.IO.File.Exists("TaskManager.db"))
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    // Create Users table
                    string createUsersTable = "CREATE TABLE Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, Username TEXT NOT NULL UNIQUE, Password TEXT NOT NULL)";
                    using (SqliteCommand command = new SqliteCommand(createUsersTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Create Tasks table
                    string createTasksTable = "CREATE TABLE Tasks (Id INTEGER PRIMARY KEY AUTOINCREMENT, Username TEXT NOT NULL, Description TEXT NOT NULL)";
                    using (SqliteCommand command = new SqliteCommand(createTasksTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static bool Login(string username, string password)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Users WHERE Username=@Username AND Password=@Password";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", Encrypt(password));

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        public static bool Register(string username, string password)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", Encrypt(password));

                    try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        public static bool CreateTask(string username, string description)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Tasks (Username, Description) VALUES (@Username, @Description)";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Description", description);

                    try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        public static List<Task> GetTasks(string username)
        {
            List<Task> tasks = new List<Task>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Tasks WHERE Username=@Username";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task task = new Task()
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Description = reader.GetString(2)
                            };
                            tasks.Add(task);
                        }
                    }
                }
            }

            return tasks;
        }

        public static Task GetTask(int taskId)
        {
            Task task = null;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Tasks WHERE Id=@Id";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", taskId);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            task = new Task()
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Description = reader.GetString(2)
                            };
                        }
                    }
                }
            }

            return task;
        }

        public static bool EditTask(int taskId, string description)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Tasks SET Description=@Description WHERE Id=@Id";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", taskId);
                    command.Parameters.AddWithValue("@Description", description);

                    try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        public static bool DeleteTask(int taskId)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Tasks WHERE Id=@Id";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", taskId);

                    try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        private static string Encrypt(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(data);
            }
            return Convert.ToBase64String(hash);
        }
    }
}
