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
                return $"{teljesNevEkezetNelkul}-{szuletesiDatum}";
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
                    .Replace('ő', 'o').Replace('Ö', 'O')
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


            class Program
            {
                static void Main(string[] args)
                {

                }
            }
        }
    }
}