using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    class Program
    {
        static void Main(string[] args)
        {
            DataBaseOperations DBOps = new DataBaseOperations();
            DBOps.ConnectToDB();
            //DBOps.CreateTable();

            Console.WriteLine("Press any key to end the program");
            Console.ReadKey();
        }
    }
}
