/*
 * ============================================================
 *  Projekt neve  : Gokart időpontfoglaló - Egyéni kisprojekt
 *  Készítette    : MCS
 *  Kezdeti dátum : 2026.09.07
 * ============================================================
 *
 *  A program egy fiktív gokart helyszín versenyzőinek generálását,
 *  és a pályabérlés alapszabályainak kezelését valósítja meg.
 *  Objektumorientált felépítés, DateTime típusok használatával.
 * ============================================================
 */

using System;
using System.Collections.Generic;
using MCS_gokart.Models;
using MCS_gokart.Services;

namespace MCS_gokart
{
    class Program
    {
        static void Main(string[] args)
        {
            // ------------------------------------------------------------
            // Fejléc kiírása a konzolra
            // ------------------------------------------------------------
            Console.WriteLine("============================================================");
            Console.WriteLine(" Projekt neve  : Gokart időpontfoglaló - Egyéni kisprojekt");
            Console.WriteLine(" Készítette    : MCS");
            Console.WriteLine(" Kezdeti dátum : 2026.09.07");
            Console.WriteLine("============================================================");
            Console.WriteLine();

            // ------------------------------------------------------------
            // 1. A gokart helyszín létrehozása és adatainak kiírása
            // ------------------------------------------------------------
            Gokartpalya palya = new Gokartpalya(
                "MCS Gokartpálya",
                "7586 Sárvár, Nagyjánosi út 46.",
                "+36-30-426-1265",
                "mcs-gokart.hu"
            );

            palya.AdatokKiirasa();
            palya.SzabalyokKiirasa();

            // ------------------------------------------------------------
            // 2. Versenyzők generálása a névfájlok alapján
            // ------------------------------------------------------------
            List<string> vezeteknevek = NevGenerator.NevekBeolvasasa("vezeteknevek.txt");
            List<string> keresztnevek = NevGenerator.NevekBeolvasasa("keresztnevek.txt");

            Random rnd = new Random();

            // Véletlenszerű, hogy hány versenyzőt generálunk (1 és 150 között)
            int versenyzoSzam = rnd.Next(1, 151); // a felső határ (151) kizárt, így max. 150 lehet

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
    }
}
