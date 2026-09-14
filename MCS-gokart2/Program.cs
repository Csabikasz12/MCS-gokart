using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace MCS_gokart2
{
    class Program
    {
        static void Main(string[] args)
        {
            public class Gokartpalya {  

            public string Nev { get; set; }
            public string Cim { get; set; }
            public string Telefonszam { get; set; }
            public string Weboldal { get; set; }


            public static readonly TimeSpan NyitasIdopont = new TimeSpan(8, 0, 0);
            public static readonly TimeSpan ZarasIdopont = new TimeSpan(19, 0, 0);

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


            }
        }
    }
}