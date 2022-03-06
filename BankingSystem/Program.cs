// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;



namespace BankingSystem
{
    class Program
    {
        static void Main(string[] args)
        {

            int choose;
            do
            {
                var logic = new Logic();
                Console.Write("if you are USER choose 1 and 2 for BANK or -1 to exit: ");
                while (!int.TryParse(Console.ReadLine(), out choose))
                {
                    Console.Write("if you are USER choose 1 and 2 for BANK or -1 to exit: ");
                }
                if (choose == -1)
                {
                    break;
                }
                if (choose == 1)
                {
                    logic.LogicForUser();
                }
                if (choose == 2)
                {
                    logic.LogicForBank();
                }
            }
            while (choose != -1);




            //var db = new DB();
            //var bank1 = new Bank("Mono", "ryskiy_korabl_idi_naxyi", 100003);
            //var bank2 = new Bank("test", "lyly", 800013);
            //var user1 = new User("test", "pass", 19, 0);

            ////db.InsertDataToBanks(ref bank2);
            //db.InsertDataToUsers(ref user1);
        }


    }
}