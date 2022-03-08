using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace BankingSystem
{
    public class Logic
    {
        DB db = new DB();
        public void LogicForUser()
        {
            int choose;
            User? currentUser = null;
            do
            {
                Console.Write("Choose 1 to login and 2 to register account and -1 to exit: ");
                while (!int.TryParse(Console.ReadLine(), out choose))
                {
                    Console.Write("Choose 1 to login and 2 to register account and -1 to exit: ");
                }
                if (choose == -1)
                {
                    return;
                }
                if (choose == 1)
                {
                    currentUser = LoginUser();
                }
                if (choose == 2)
                {
                    currentUser = RegisterUser();
                }
                if (currentUser != null)
                    break;
            }
            while (choose != -1);

            List<Bank> banksList = new List<Bank>();
            var sqlBankReader = db.SelectDataFromBanks();
            int i, bankNumber;
            bool cbflag = false, exit = false;
            while (sqlBankReader.Read())
            {
                var temp = new Bank(sqlBankReader["Name"].ToString(),
                    sqlBankReader["Password"].ToString(),
                    Convert.ToDouble(sqlBankReader["Balance"]));
                banksList.Add(temp);
            }


            while (!exit)
            {
                cbflag = false;
                while (true)
                {
                    for (i = 1; i <= banksList.Count; i++)
                    {
                        Console.WriteLine($" >{i} {banksList[i - 1].Name}");
                    }

                    Console.Write("Choose a  bank from the above. (number): ");
                    while (!int.TryParse(Console.ReadLine(), out bankNumber))
                    {
                        Console.Write("Choose a  bank from the above. (number): ");
                    }
                    if (bankNumber <= 0 || bankNumber >= i)
                    {
                        Console.WriteLine("Invalid Bank...");
                    }
                    else
                    {
                        break;
                    }
                }
                --bankNumber;
                Bank currentBank = banksList[bankNumber];
                Console.WriteLine("names of available operations:\n" +
                    " ·loan - to take loan\n" +
                    " ·dep - to give a deposit\n" +
                    " ·bal - to find out your and bank's balance\n" +
                    " ·exit - to exit\n" +
                    " ·cb - to change bank");
                //CHANGE BANK
                Console.WriteLine($"~The beginning of the transaction history with {currentBank.Name}~");
                while (true)
                {
                    string op = Console.ReadLine();
                    switch (op)
                    {
                        case "exit":
                            exit = true;
                            break;
                        case "loan":
                            TakeLoan(ref currentUser, ref currentBank);
                            break;
                        case "dep":
                            AcceptanceOfDeposits(ref currentUser, ref currentBank);
                            break;
                        case "bal":
                            GetBallance(ref currentUser, ref currentBank);
                            break;
                        case "cb":
                            cbflag = true;
                            break;
                        default:
                            break;
                    }
                    if (cbflag || exit)
                    {
                        banksList[bankNumber] = currentBank;
                        break;
                    }
                }
                Console.WriteLine("~End of transaction history~");
            }

            db.UpdateDataInUsers(ref currentUser);
            db.UpdateDataInBanks(ref banksList);

            return;
        }
        private User LoginUser()
        {
            string? name, password;
            System.Console.Write("Enter your name: ");
            name = System.Console.ReadLine();
            while (name == "")
            {
                System.Console.Write("Enter your name: ");
                name = System.Console.ReadLine();
            }

            System.Console.Write("Enter your password: ");
            password = System.Console.ReadLine();
            while (password == "")
            {
                System.Console.Write("Enter your password: ");
                password = System.Console.ReadLine();
            }

            var sqlDataReader = db.SelectDataFromUsers();
            while (sqlDataReader.Read())
            {
                if (Convert.ToString(sqlDataReader["Name"]) == name && Convert.ToString(sqlDataReader["Password"]) == password)
                {
                    Console.WriteLine("Successfully logined in :)");

                    User? temp = new(Convert.ToString(sqlDataReader["Name"]), Convert.ToString(sqlDataReader["Password"]), Convert.ToInt32(sqlDataReader["Age"]), Convert.ToDouble(sqlDataReader["Balance"]));
                    return temp;
                }
            }
            Console.WriteLine("Account not found :(");
            return null;
        }

        private User RegisterUser()
        {

            string? name, password;
            int age;
            System.Console.Write("Enter your name: ");
            name = System.Console.ReadLine();
            while (name == "")
            {
                System.Console.Write("Enter your name: ");
                name = System.Console.ReadLine();
            }

            System.Console.Write("Enter your password: ");
            password = System.Console.ReadLine();
            while (password == "")
            {
                System.Console.Write("Enter your password: ");
                password = System.Console.ReadLine();
            }

            System.Console.Write("Enter your age: ");
            while (!int.TryParse(Console.ReadLine(), out age) && age < 0)
            {
                System.Console.Write("Enter your age: ");
            }

            var sqlDataReader = db.SelectDataFromUsers();

            while (sqlDataReader.Read())
            {
                if (Convert.ToString(sqlDataReader["Name"]) == name)
                {
                    Console.WriteLine("User with this name is already existed. Try another name!");
                    return null;
                }
            }

            User temp = new User(name, password, age, 0);

            db.InsertDataToUsers(ref temp);
            return temp;
        }


        private void AcceptanceOfDeposits(ref User user, ref Bank bank)
        {
            double deposit;

            Console.Write("    How much money you want to place in deposit: ");
            while (!double.TryParse(Console.ReadLine(), out deposit) && deposit < 0)
            {
                Console.Write("    How much money you want to place in deposit: ");
            }
            if (user.Balance < deposit)
            {
                Console.WriteLine("    You don't have enough money!");
            }
            else
            {
                user.Balance -= deposit/2;
                bank.Balance += deposit;
                Console.WriteLine("    Successful deposit!");
            }
        }

        private void TakeLoan(ref User user, ref Bank bank)
        {
            double loan;

            Console.Write("    how much money you want to lend: ");
            while (!double.TryParse(Console.ReadLine(), out loan) && loan < 0)
            {
                Console.Write("    how much money you want to lend: ");
            }
            if (bank.Balance < loan)
            {
                Console.WriteLine("    Bank doesn't have enough money!");
            }
            else
            {
                user.Balance += loan;
                bank.Balance -= loan/3;
                Console.WriteLine("    Successful loan!");
            }
        }

        private void GetBallance(ref User user, ref Bank bank)
        {
            Console.WriteLine($"    Your money: {user.Balance}");
            Console.WriteLine($"    {bank.Name}'s money: {bank.Balance}");
        }
        private void GetBallance(ref Bank bank)
        {
            Console.WriteLine($"    {bank.Name}'s money: {bank.Balance}");
        }

        public void LogicForBank()
        {
            int choose;
            Bank? currentBank = null;
            do
            {
                Console.Write("Choose 1 to login and 2 to register account and -1 to exit: ");
                while (!int.TryParse(Console.ReadLine(), out choose))
                {
                    Console.Write("Choose 1 to login and 2 to register account and -1 to exit: ");
                }
                if (choose == -1)
                {
                    return;
                }
                if (choose == 1)
                {
                    currentBank = LoginBank();
                }
                if (choose == 2)
                {
                    currentBank = RegisterBank();
                }
                if (currentBank != null)
                    break;
            }
            while (choose != -1);


            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("names of available operations:\n" +
                    " ·tu - to top up your account\n" +
                    " ·bal - to find out your and bank's balance\n" +
                    " ·exit - to exit\n");

                Console.WriteLine($"~The beginning of {currentBank.Name}'s operations history~");
                while (true)
                {
                    string op = Console.ReadLine();
                    switch (op)
                    {
                        case "exit":
                            exit = true;
                            break;
                        case "tu":
                            TopUpBalance(ref currentBank);
                            break;
                        case "bal":
                            GetBallance(ref currentBank);
                            break;
                        default:
                            break;
                    }
                    if (exit)
                    {
                        break;
                    }
                }
                Console.WriteLine("~End of bank history~");
            }

            db.UpdateBank(ref currentBank);
        }

        private Bank LoginBank()
        {
            string? name, password;

            System.Console.Write("Enter your name: ");
            name = System.Console.ReadLine();
            while (name == "")
            {
                System.Console.Write("Enter your name: ");
                name = System.Console.ReadLine();
            }

            System.Console.Write("Enter your password: ");
            password = System.Console.ReadLine();
            while (password == "")
            {
                System.Console.Write("Enter your password: ");
                password = System.Console.ReadLine();
            }

            var sqlDataReader = db.SelectDataFromBanks();

            while (sqlDataReader.Read())
            {
                if (Convert.ToString(sqlDataReader["Name"]) == name && Convert.ToString(sqlDataReader["Password"]) == password)
                {
                    Console.WriteLine("Successfully logined in :)");

                    Bank? temp = new(Convert.ToString(sqlDataReader["Name"]), Convert.ToString(sqlDataReader["Password"]), Convert.ToDouble(sqlDataReader["Balance"]));
                    return temp;
                }
            }

            Console.WriteLine("Account not found :(");
            return null;
        }

        private Bank RegisterBank()
        {

            string? name, password;

            System.Console.Write("Enter your name: ");
            name = System.Console.ReadLine();
            while (name == "")
            {
                System.Console.Write("Enter your name: ");
                name = System.Console.ReadLine();
            }

            System.Console.Write("Enter your password: ");
            password = System.Console.ReadLine();
            while (password == "")
            {
                System.Console.Write("Enter your password: ");
                password = System.Console.ReadLine();
            }

            var sqlDataReader = db.SelectDataFromBanks();

            while (sqlDataReader.Read())
            {
                if (Convert.ToString(sqlDataReader["Name"]) == name)
                {
                    Console.WriteLine("User with this name is already existed. Try another name!");
                    return null;
                }
            }

            Bank temp = new Bank(name, password, 0);

            db.InsertDataToBanks(ref temp);
            return temp;
        }

        private void TopUpBalance(ref Bank bank)
        {
            double bal;
            Console.Write("    how much money you want to top up: ");
            while (!double.TryParse(Console.ReadLine(), out bal))
            {
                Console.Write("    how much money you want to top up: ");
            }
            if (bal < 0)
            {
                Console.WriteLine("    Invalid amount of money!");
            }
            else
            {
                bank.Balance += bal;
                Console.WriteLine("    Successful replenishment!");
            }
        }
    }

}