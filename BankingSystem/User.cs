using System;

namespace BankingSystem
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public double Balance { get; set; }

        public User(string name, string password, int age, double balance)
        {
            Name = name;
            Password = password;
            Age = age;
            Balance = balance;
        }

        //public void CreateUser()
        //{
        //    Console.WriteLine("Enter name: ");
        //    m_name = Console.ReadLine();

        //}
        //public bool LoginUser(string name, string password)
        //{.WriteLine("Enter Deposit Amount:");

        //    return false;
        //}
        //public void Deposit()
        //{
        //    Console
        //    m_deposit = Convert.ToDouble(Console.ReadLine());
        //    m_balance += m_deposit;
        //}
    }
}
