using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SchoolProject.SchoolDatabase;

namespace SchoolProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SchoolDatabase school = new SchoolDatabase(connectionString);
            string command = Console.ReadLine();

            switch (command)
            {
                case "create":
                    school.CreateDatabaseAndTables();
                    break;

                case "insert":
                    school.InsertData();
                    break;

                case "query":
                    int num = int.Parse(Console.ReadLine());
                    school.SelectQuery(num);
                    break;
                case "log":
                    string text=Console.ReadLine();
                    LogForExcel logForExcel = new LogForExcel();
                    logForExcel.WriteToWorkbook(text);
                    break;

                default:
                    Console.WriteLine("Invalid command! Try again!!! ");
                    break;
            }
        }
    }
}
