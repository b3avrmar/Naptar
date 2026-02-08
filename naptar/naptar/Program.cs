using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace naptar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Esemeny> esemenyek = Esemenyek_betoltese();

            string[,] naptar = Naptar_generalas();
            Menu(naptar, esemenyek);
        }

        public struct Esemeny
        {
            public string Nev;
            public DateTime Datum;
            public TimeSpan Idotartam;
            public string Leiras;
            public string Felhasznalo;

        }

        static List<Esemeny> Esemenyek_betoltese()
        {
            List<Esemeny> esemenyek = new List<Esemeny>();
            if (File.Exists("esemenyek.csv"))
            {
                string[] lines = File.ReadAllLines("esemenyek.csv");
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length == 5)
                    {
                        Esemeny esemeny = new Esemeny
                        {
                            Nev = parts[0],
                            Datum = DateTime.Parse(parts[1]),
                            Idotartam = TimeSpan.Parse(parts[2]),
                            Leiras = parts[3],
                            Felhasznalo = parts[4]
                        };
                        esemenyek.Add(esemeny);
                    }
                }
            }
            return esemenyek;
        }

        static void Menu(string[,] naptar, List<Esemeny> esemenyek)
        {
            Console.WriteLine("1. Naptár megjelenítése");
            Console.WriteLine("2. Esemény hozzáadása");
            Console.WriteLine("3. Legközelebbi esemány megjelenítése");
            Console.WriteLine("4. Kilépés");

            Console.Write("Válasszon egy opciót: ");
            int input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:
                    Naptar_megjelenites(naptar);
                    break;
                case 2:
                    Esemeny_hozzaadas(esemenyek);
                    break;
                case 3:
                    Legkozelebbi_esemeny(esemenyek);
                    break;
                case 4:
                    break;
                default:
                    Console.WriteLine("Hibás opció, próbálja újra.");
                    Console.ReadKey();
                    Console.Clear();
                    Menu(naptar, esemenyek);
                    break;
            }
        }

        static string[,] Naptar_generalas()
        {
            string[,] naptar = new string[6, 7];
            naptar[0, 0] = "H";
            naptar[0, 1] = "K";
            naptar[0, 2] = "Sz";
            naptar[0, 3] = "Cs";
            naptar[0, 4] = "P";
            naptar[0, 5] = "Szo";
            naptar[0, 6] = "V";
            int day = 0;
            for (int i = 1; i < naptar.GetLength(0); i++)
            {
                for (int j = 0; j < naptar.GetLength(1); j++)
                {
                    if (day > 29)
                    {
                        break;
                    }
                    else if (day != 0)
                    {
                        naptar[i, j] = Convert.ToString(day);
                        day++;
                    }
                    else { day++; }
                }
            }
            return naptar;
        }
        static void Naptar_megjelenites(string[,] naptar)
        {
            for (int i = 0; i < naptar.GetLength(0); i++)
            {
                for (int j = 0; j < naptar.GetLength(1); j++)
                {
                    Console.Write(naptar[i, j] + "\t");
                }
                Console.WriteLine();
            }

        }

        static List<Esemeny> Esemeny_hozzaadas(List<Esemeny> esemenyek)
        {
            Console.Write("Add meg space-el elválasztva az esemény nevét, dátumát, időtartamát, leírását és a felhasználó nevét: ");
            string[] input = Console.ReadLine().Split(' ');

            Esemeny ujEsemeny = new Esemeny
            {
                Nev = input[0],
                Datum = DateTime.Parse(input[1]),
                Idotartam = TimeSpan.Parse(input[2]),
                Leiras = input[3],
                Felhasznalo = input[4]
            };

            esemenyek.Add(ujEsemeny);
            Mentes(esemenyek);
            return esemenyek;
        }

        static void Legkozelebbi_esemeny(List<Esemeny> esemenyek)
        {
            Random rnd = new Random();
            int nap = rnd.Next(1, 30);
            DateTime date = new DateTime(2028, 2, nap);

            Console.WriteLine($"Keresett dátum: {date.ToShortDateString()}");

            if (esemenyek.Count == 0)
            {
                Console.WriteLine("Nincsenek események a listában.");
                return;
            }

            Esemeny legkozelebbi = esemenyek[0];
            TimeSpan legkisebbKulonbseg = TimeSpan.MaxValue;

            foreach (Esemeny esemeny in esemenyek)
            {
                TimeSpan kulonbseg = (esemeny.Datum - date).Duration();

                if (kulonbseg < legkisebbKulonbseg)
                {
                    legkisebbKulonbseg = kulonbseg;
                    legkozelebbi = esemeny;
                }
            }

            Console.WriteLine($"\nLegközelebbi esemény:");
            Console.WriteLine($"Név: {legkozelebbi.Nev}");
            Console.WriteLine($"Dátum: {legkozelebbi.Datum.ToShortDateString()}");
            Console.WriteLine($"Különbség: {legkisebbKulonbseg.Days} nap");
        }

        static void Mentes(List<Esemeny> esemenyek)
        {
            StreamWriter sw = new StreamWriter("esemenyek.csv");
            foreach (var item in esemenyek)
            {
                sw.WriteLine($"{item.Nev};{item.Datum};{item.Idotartam};{item.Leiras};{item.Felhasznalo}");
            }
            sw.Flush();
            sw.Close();
        }
    }
}
