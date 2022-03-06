using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem
{
    public class Bank
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public double Balance { get; set; }

        public Bank(string name, string password, double balance)
        {
            Name = name;
            Password = password;
            Balance = balance;
        }
    }
}
