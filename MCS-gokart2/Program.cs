using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace MCS_gokart2
{
    public class Gokartpalya
    {
        public string Nev { get; set; }
        public string Cim { get; set; }
        public string Telefonszam { get; set; }
        public string Weboldal { get; set; }

        public static readonly TimeSpan NyitasIdopont = new TimeSpan(8, 0, 0);
        public static readonly TimeSpan ZarasIdopont = new TimeSpan(19, 0, 0);

        public const int MinBerlesIdotartamOra = 1;
        public const int MaxBerlesIdotartamOra = 2;

        public const int MinVersenyzoLetszam = 8;
        public const int MaxVersenyzoLetszam = 20;

        public Gokartpalya(string nev, string cim, string telefonszam, string weboldal)
        {
            Nev = nev;
            Cim = cim;
            Telefonszam = telefonszam;
            Weboldal = weboldal;
        }

        public void AdatokKiirasa()
        {
            Console.WriteLine("===Gokart helyszín adatai===");
            Console.WriteLine($"Név: {Nev}");
            Console.WriteLine($"Cím: {Cim}");
            Console.WriteLine($"Telefonszám: {Telefonszam}");
            Console.WriteLine($"Weboldal: {Weboldal}");
            Console.WriteLine();
        }

        public void SzabalyokKiirasa()
        {
            Console.WriteLine("=== Pályabérlés szabályai ===");
            Console.WriteLine($"Nyitvatartás      : {NyitasIdopont:hh\\:mm} - {ZarasIdopont:hh\\:mm}");
            Console.WriteLine($"Min. bérlési idő  : {MinBerlesIdotartamOra} óra / fő");
            Console.WriteLine($"Max. bérlési idő  : {MaxBerlesIdotartamOra} óra / fő (összefüggő)");
            Console.WriteLine($"Min. létszám      : {MinVersenyzoLetszam} fő / menet");
            Console.WriteLine($"Max. létszám      : {MaxVersenyzoLetszam} fő / menet");
            Console.WriteLine();
        }
    } // <-- Gokartpalya vége


    public class Versenyzo
    {
        public string Vezeteknev { get; private set; }
        public string Keresztnev { get; private set; }
        public DateTime SzuletesiIdo { get; private set; }
        public string VersenyzoAzonosito { get; private set; }
        public string Email { get; private set; }

        public bool Elmult18Eves
        {
            get
            {
                DateTime ma = DateTime.Today;
                int eletkor = ma.Year - SzuletesiIdo.Year;

                if (SzuletesiIdo.Date > ma.AddYears(-eletkor))
                {
                    eletkor--;
                }
                return eletkor >= 18;
            }
        }

        public Versenyzo(string vezeteknev, string keresztnev, DateTime szuletesiIdo)
        {
            Vezeteknev = vezeteknev;
            Keresztnev = keresztnev;
            SzuletesiIdo = szuletesiIdo;
            VersenyzoAzonosito = AzonositoGeneralasa();
            Email = EmailGeneralasa();
        }

        private string AzonositoGeneralasa()
        {
            string teljesNevEkezetNelkul = EkezetEltavolitasa(Vezeteknev + Keresztnev);
            string szuletesiDatum = SzuletesiIdo.ToString("yyyyMMdd");
            return $"GO-{teljesNevEkezetNelkul}-{szuletesiDatum}";
        }

        private string EmailGeneralasa()
        {
            string vezeteknevTiszta = EkezetEltavolitasa(Vezeteknev).ToLower();
            string keresztnevTiszta = EkezetEltavolitasa(Keresztnev).ToLower();
            return $"{vezeteknevTiszta}.{keresztnevTiszta}@gmail.com";
        }

        private static string EkezetEltavolitasa(string szoveg)
        {
            string normalizalt = szoveg.Normalize(NormalizationForm.FormD);
            StringBuilder eredmeny = new StringBuilder();

            foreach (char karakter in normalizalt)
            {
                UnicodeCategory kategoria = CharUnicodeInfo.GetUnicodeCategory(karakter);
                if (kategoria != UnicodeCategory.NonSpacingMark)
                {
                    eredmeny.Append(karakter);
                }
            }
            return eredmeny.ToString()
                .Normalize(NormalizationForm.FormC)
                .Replace('ő', 'o').Replace('Ő', 'O')
                .Replace('ű', 'u').Replace('Ű', 'U');
        }

        public void AdatokKiirasa()
        {
            Console.WriteLine($"Név              : {Vezeteknev} {Keresztnev}");
            Console.WriteLine($"Születési idő    : {SzuletesiIdo:yyyy.MM.dd}");
            Console.WriteLine($"Elmúlt 18 éves   : {Elmult18Eves}");
            Console.WriteLine($"Versenyző-azon.  : {VersenyzoAzonosito}");
            Console.WriteLine($"Email cím        : {Email}");
        }
    } // <-- Versenyzo vége


    public static class NevGenerator
    {
        public static List<string> NevekBeolvasasa(string fajlNev)
        {
            List<string> nevek = new List<string>();
            string teljesUtvonal = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fajlNev);

            if (!File.Exists(teljesUtvonal))
            {
                Console.WriteLine($"HIBA: A(z) '{fajlNev}' fájl nem található itt: {teljesUtvonal}");
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
            return nevek;
        }

        public static Versenyzo VeletlenVersenyzoGeneralasa(List<string> vezeteknevek, List<string> keresztnevek, Random rnd)
        {
            string vezeteknev = vezeteknevek[rnd.Next(vezeteknevek.Count)];
            string keresztnev = keresztnevek[rnd.Next(keresztnevek.Count)];

            DateTime minDatum = new DateTime(1950, 1, 1);
            DateTime maxDatum = DateTime.Today.AddYears(-1);
            int napTartomany = (maxDatum - minDatum).Days;
            DateTime szuletesiIdo = minDatum.AddDays(rnd.Next(napTartomany));

            return new Versenyzo(vezeteknev, keresztnev, szuletesiIdo);
        }
    } // <-- NevGenerator vége


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("============================================================");
            Console.WriteLine(" Projekt neve  : Gokart időpontfoglaló - Egyéni kisprojekt");
            Console.WriteLine(" Készítette    : Mészáros Csaba");
            Console.WriteLine(" Kezdeti dátum : 2026.09.07");
            Console.WriteLine("============================================================");
            Console.WriteLine();

            Gokartpalya palya = new Gokartpalya(
                "Ampax Gokartpálya",
                "1117 Budapest, Budafoki út 183.",
                "+36 1 123 4567",
                "ampaxgokart.hu"
            );

            palya.AdatokKiirasa();
            palya.SzabalyokKiirasa();

            List<string> vezeteknevek = NevGenerator.NevekBeolvasasa("vezeteknevek.txt");
            List<string> keresztnevek = NevGenerator.NevekBeolvasasa("keresztnevek.txt");

            Random rnd = new Random();
            int versenyzoSzam = rnd.Next(1, 151);

            Console.WriteLine($"Generált versenyzők száma: {versenyzoSzam}");
            Console.WriteLine("------------------------------------------------------------");

            List<Versenyzo> versenyzok = new List<Versenyzo>();

            for (int i = 0; i < versenyzoSzam; i++)
            {
                Versenyzo ujVersenyzo = NevGenerator.VeletlenVersenyzoGeneralasa(vezeteknevek, keresztnevek, rnd);
                versenyzok.Add(ujVersenyzo);

                ujVersenyzo.AdatokKiirasa();
                Console.WriteLine("------------------------------------------------------------");
            }

            Console.WriteLine();
            Console.WriteLine($"Összesen {versenyzok.Count} versenyző került generálásra.");

            Console.WriteLine();
            Console.WriteLine("Nyomj meg egy billentyűt a kilépéshez...");
            Console.ReadKey();
        }
    } // <-- Program vége

} // <-- namespace vége