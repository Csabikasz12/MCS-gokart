using System;
using System.Collections.Generic;
using System.IO;
using MCS_gokart.Models;

namespace MCS_gokart.Services
{
    /// <summary>
    /// Segédosztály a vezeteknevek.txt / keresztnevek.txt fájlok
    /// beolvasásához, és ezekből véletlenszerű Versenyzo objektumok
    /// generálásához.
    /// </summary>
    public static class NevGenerator
    {
        /// <summary>
        /// Beolvassa a megadott nevű txt fájlt a program futtatási
        /// könyvtárából (pl. bin/Debug/net8.0), és soronként egy-egy
        /// névként adja vissza a tartalmát.
        /// </summary>
        public static List<string> NevekBeolvasasa(string fajlNev)
        {
            List<string> nevek = new List<string>();
            string teljesUtvonal = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fajlNev);

            if (!File.Exists(teljesUtvonal))
            {
                Console.WriteLine($"HIBA: A(z) '{fajlNev}' fájl nem található itt: {teljesUtvonal}");
                Console.WriteLine("Ellenőrizd, hogy a fájl a projekt gyökerében van-e, és a");
                Console.WriteLine("'Copy to Output Directory' beállítása 'Copy if newer'-re van állítva.");
                Environment.Exit(1);
            }

            string[] sorok = File.ReadAllLines(teljesUtvonal);
            foreach (string sor in sorok)
            {
                string tisztitottSor = sor.Trim();
                if (!string.IsNullOrWhiteSpace(tisztitottSor))
                {
                    nevek.Add(tisztitottSor);
                }
            }

            if (nevek.Count == 0)
            {
                Console.WriteLine($"HIBA: A(z) '{fajlNev}' fájl üres!");
                Environment.Exit(1);
            }

            return nevek;
        }

        /// <summary>
        /// Egy véletlenszerű Versenyzo objektumot generál a megadott
        /// névlisták és Random példány alapján.
        /// </summary>
        public static Versenyzo VeletlenVersenyzoGeneralasa(
            List<string> vezeteknevek,
            List<string> keresztnevek,
            Random rnd)
        {
            string vezeteknev = vezeteknevek[rnd.Next(vezeteknevek.Count)];
            string keresztnev = keresztnevek[rnd.Next(keresztnevek.Count)];

            // Véletlenszerű születési dátum: 1950.01.01 és a mai nap
            // (1 évvel korábbi dátuma) között.
            DateTime minDatum = new DateTime(1950, 1, 1);
            DateTime maxDatum = DateTime.Today.AddYears(-1);

            int napTartomany = (maxDatum - minDatum).Days;
            DateTime szuletesiIdo = minDatum.AddDays(rnd.Next(napTartomany));

            return new Versenyzo(vezeteknev, keresztnev, szuletesiIdo);
        }
    }
}
