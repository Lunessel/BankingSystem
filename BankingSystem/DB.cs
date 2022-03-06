using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace BankingSystem
{
    internal class DB
    {
        private SqlConnection sqlconnection = null;
        SqlDataReader sqlDataReader = null;

        public DB()
        {
        }
        ~DB()
        {
        }
        private void OpenDB()
        {
            try
            {
                sqlconnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString);
                sqlconnection.Open();
                if (sqlconnection.State == ConnectionState.Open)
                {
                    //Console.WriteLine("Successfully opened database!");
                    return;
                }
                Console.WriteLine("Failed database opening...");
                return;
            }
            catch
            {
                Console.WriteLine("Failed database opening...");
                return;
            }
        }

        private void CloseDB()
        {
            sqlconnection.Close();
        }
        public SqlDataReader? SelectDataFromUsers()
        {
            try
            {
                OpenDB();
                string command = "SELECT * FROM Users";
                SqlCommand sqlCommand = new SqlCommand(command, sqlconnection);

                return sqlCommand.ExecuteReader();
            }
            catch
            {
                Console.WriteLine("Failed selecting from Table Users");
                return null;
            }

        }      
        public void InsertDataToUsers(ref User user)
        {
            OpenDB();
            try
            {
                string command = $"INSERT INTO Users (Name, Password, Age, Balance) VALUES (@Name, @Password, @Age, @Balance)";
                using (SqlCommand sqlCommand = new SqlCommand(command, sqlconnection))
                {
                    sqlCommand.Parameters.Add("@Name", SqlDbType.Text).Value = user.Name;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.Text).Value = user.Password;
                    sqlCommand.Parameters.Add("@Age", SqlDbType.Int).Value = user.Age;
                    sqlCommand.Parameters.Add("@Balance", SqlDbType.Real).Value = user.Balance;


                    int result = sqlCommand.ExecuteNonQuery();

                    // Check Error
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Users!");
                }
            }
            catch
            {
                Console.WriteLine("Error inserting data into Users!");
                return;
            }
        }

        public void InsertDataToBanks(ref Bank bank)
        {
            OpenDB();
            try
            {
                string command = $"INSERT INTO Banks (Name, Password, Balance) VALUES (@Name, @Password, @Balance)";
                using (SqlCommand sqlCommand = new SqlCommand(command, sqlconnection))
                {
                    sqlCommand.Parameters.Add("@Name", SqlDbType.Text).Value = bank.Name;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.Text).Value = bank.Password;
                    sqlCommand.Parameters.Add("@Balance", SqlDbType.Real).Value = bank.Balance;


                    int result = sqlCommand.ExecuteNonQuery();

                    // Check Error
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Banks!");
                }
            }
            catch
            {
                Console.WriteLine("Error inserting data into Banks!");
                return;
            }
        }


        public SqlDataReader? SelectDataFromBanks()
        {
            try
            {
                OpenDB();
                string command = "SELECT * FROM Banks";
                SqlCommand sqlCommand = new SqlCommand(command, sqlconnection);

                return sqlCommand.ExecuteReader();
            }
            catch
            {
                Console.WriteLine("Failed selecting from Table Banks");
                return null;
            }
            return null;
        }

        public void UpdateDataInUsers(ref User user)
        {
            OpenDB();
            string command = $"UPDATE Users SET Balance=@Balance WHERE Name LIKE @Name;";
            using (SqlCommand sqlCommand = new SqlCommand(command, sqlconnection))
            {
                sqlCommand.Parameters.Add("@Balance", SqlDbType.Real).Value = user.Balance;
                sqlCommand.Parameters.Add("@Name", SqlDbType.Text).Value = user.Name;

                sqlCommand.ExecuteNonQuery();

            }

        }

        public void UpdateDataInBanks(ref List<Bank> banksList)
        {
            OpenDB();
            foreach (var bank in banksList)
            {
                string command = $"UPDATE Banks SET Balance=@Balance WHERE Name LIKE @Name;";
                using (SqlCommand sqlCommand = new SqlCommand(command, sqlconnection))
                {
                    sqlCommand.Parameters.Add("@Balance", SqlDbType.Real).Value = bank.Balance;
                    sqlCommand.Parameters.Add("@Name", SqlDbType.Text).Value = bank.Name;

                    sqlCommand.ExecuteNonQuery();

                }
            }
        }
        public void UpdateBank(ref Bank bank)
        {
            OpenDB();
            string command = $"UPDATE Banks SET Balance=@Balance WHERE Name LIKE @Name;";
            using (SqlCommand sqlCommand = new SqlCommand(command, sqlconnection))
            {
                sqlCommand.Parameters.Add("@Balance", SqlDbType.Real).Value = bank.Balance;
                sqlCommand.Parameters.Add("@Name", SqlDbType.Text).Value = bank.Name;

                sqlCommand.ExecuteNonQuery();

            }
        }
    
    }

}
