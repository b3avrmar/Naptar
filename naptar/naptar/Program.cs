using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static naptar.Program;

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
            if (File.Exists("data.csv"))
            {
                foreach (string line in File.ReadAllLines("data.csv").Skip(1))
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
            Console.WriteLine("Új esemény hozzáadása");
            Console.WriteLine("Kérlek add meg az alábbi adatokat SZÓKÖZZEL elválasztva, EGY SORBAN:");
            Console.WriteLine("1. Esemény neve (pl: Meeting)");
            Console.WriteLine("2. Nap száma 1-29 között (pl: 15)");
            Console.WriteLine("3. Kezdési időpont óra:perc formátumban 8:00-19:59 között (pl: 14:30)");
            Console.WriteLine("4. Időtartam percben 30-120 között (pl: 60)");
            Console.WriteLine("5. Leírás (pl: Fontos-megbeszeles)");
            Console.WriteLine("6. Felhasználó neve (pl: Kovacs)");
            Console.WriteLine();
            Console.WriteLine("Példa: Meeting 15 14:30 60 Fontos-megbeszeles anya/apa");
            Console.WriteLine();
            Console.Write("Input mező: ");

            string[] input = Console.ReadLine().Split(' ');

            int nap = int.Parse(input[1]);
            string[] idopont = input[2].Split(':');
            int ora = int.Parse(idopont[0]);
            int perc = int.Parse(idopont[1]);
            int percek = int.Parse(input[3]);

            DateTime datum = new DateTime(2028, 2, nap, ora, perc, 0);
            TimeSpan idotartam = TimeSpan.FromMinutes(percek);

            if (ora < 8 || ora >= 20)
            {
                Console.WriteLine("Hiba: Az esemény csak reggel 8 és este 20 óra között vehető fel!");
                return esemenyek;
            }

            if (idotartam < TimeSpan.FromMinutes(30) || idotartam > TimeSpan.FromHours(2))
            {
                Console.WriteLine("Hiba: Az esemény időtartama csak 30 perc és 2 óra között lehet!");
                return esemenyek;
            }

            Esemeny ujEsemeny = new Esemeny
            {
                Nev = input[0],
                Datum = datum,
                Idotartam = idotartam,
                Leiras = input[4],
                Felhasznalo = input[5]
            };

            esemenyek.Add(ujEsemeny);
            Mentes(esemenyek);

            Console.WriteLine("Esemény sikeresen hozzáadva!");
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
            Console.WriteLine($"Időtartam: {legkozelebbi.Idotartam}");
            Console.WriteLine($"Leírás: {legkozelebbi.Leiras}");
            Console.WriteLine($"Felhasználó: {legkozelebbi.Felhasznalo}");
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
