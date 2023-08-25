using Microsoft.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleApp42
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Enter database name: ");
            string dbName = Console.ReadLine();

            string connectionString = @"Data Source = DESKTOP-2J3MN6S; Initial Catalog = master; Trusted_Connection=True; TrustServerCertificate= True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Создание базы данных
                SqlCommand createDbCommand = new SqlCommand($"CREATE DATABASE {dbName}", connection);
                await createDbCommand.ExecuteNonQueryAsync();
                Console.WriteLine($"Database '{dbName}' Created");

                // Переключение на новую базу данных
                connection.ChangeDatabase(dbName);

                // Создание таблицы
                Console.Write("Enter table name: ");
                string tableName = Console.ReadLine();

                Console.Write("Enter number of columns: ");
                int columnCount = int.Parse(Console.ReadLine());

                SqlCommand createTableCommand = new SqlCommand();
                createTableCommand.Connection = connection;
                createTableCommand.CommandText = $"CREATE TABLE {tableName} (";

                for (int i = 0; i < columnCount; i++)
                {
                    Console.Write($"Enter name for column {i + 1}: ");
                    string columnName = Console.ReadLine();

                    Console.Write($"Enter data type for {columnName}: ");
                    string dataType = Console.ReadLine();

                    createTableCommand.CommandText += $"{columnName} {dataType}";

                    if (i < columnCount - 1)
                    {
                        createTableCommand.CommandText += ", ";
                    }
                }

                createTableCommand.CommandText += ")";
                await createTableCommand.ExecuteNonQueryAsync();
                Console.WriteLine("Table Created");

                Console.WriteLine($"Table '{tableName}' has been created with {columnCount} columns.");

                // Наполнение таблицы
                while (true)
                {
                    Console.WriteLine("Enter data for the table (or type 'exit' to finish): ");
                    string input = Console.ReadLine();

                    if (input.ToLower() == "exit")
                        break;

                    SqlCommand insertCommand = new SqlCommand($"INSERT INTO {tableName} VALUES ({input})", connection);
                    await insertCommand.ExecuteNonQueryAsync();
                    Console.WriteLine("Row Inserted");
                }
            }
        }
    }
}