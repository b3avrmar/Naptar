using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace naptar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Felhasznalo();
        }

        static string Felhasznalo()
        {
            while (true)
            {
                Console.Write("Adja meg a felhasználot: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "Apa":
                        return "Apa";
                    case "Anya":
                        return "Anya";
                    default:
                        Console.WriteLine("Hibás felhasználó, próbálja újra.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }

        public struct Datetime
        {
            
        }
    }
}
