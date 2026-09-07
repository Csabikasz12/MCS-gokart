using System;

namespace MCS_gokart.Models
{
    /// <summary>
    /// A fiktív gokart helyszín adatait és a pályabérlés szabályait
    /// tartalmazó osztály.
    /// </summary>
    public class Gokartpalya
    {
        public string Nev { get; set; }
        public string Cim { get; set; }
        public string Telefonszam { get; set; }
        public string Weboldal { get; set; }

        // ------------------------------------------------------------
        // A pályabérlés szabályai (állandó, változtathatatlan értékek)
        // ------------------------------------------------------------
        public static readonly TimeSpan NyitasIdopont = new TimeSpan(8, 0, 0);   // 8:00
        public static readonly TimeSpan ZarasIdopont = new TimeSpan(19, 0, 0);   // 19:00

        public const int MinBerlesIdotartamOra = 1;    // minimum 1 óra bérlés / fő
        public const int MaxBerlesIdotartamOra = 2;    // maximum 2 óra bérlés / fő (összefüggő)

        public const int MinVersenyzoLetszam = 8;      // pályán egyszerre min. 8 fő
        public const int MaxVersenyzoLetszam = 20;     // pályán egyszerre max. 20 fő

        public Gokartpalya(string nev, string cim, string telefonszam, string weboldal)
        {
            Nev = nev;
            Cim = cim;
            Telefonszam = telefonszam;
            Weboldal = weboldal;
        }

        /// <summary>
        /// A helyszín alapadatainak kiírása a konzolra.
        /// </summary>
        public void AdatokKiirasa()
        {
            Console.WriteLine("=== Gokart helyszín adatai ===");
            Console.WriteLine($"Név          : {Nev}");
            Console.WriteLine($"Cím          : {Cim}");
            Console.WriteLine($"Telefonszám  : {Telefonszam}");
            Console.WriteLine($"Weboldal     : {Weboldal}");
            Console.WriteLine();
        }

        /// <summary>
        /// A pályabérlés szabályainak kiírása a konzolra.
        /// </summary>
        public void SzabalyokKiirasa()
        {
            Console.WriteLine("=== Pályabérlés szabályai ===");
            Console.WriteLine($"Nyitvatartás      : {NyitasIdopont:hh\\:mm} - {ZarasIdopont:hh\\:mm}");
            Console.WriteLine($"Min. bérlési idő  : {MinBerlesIdotartamOra} óra / fő");
            Console.WriteLine($"Max. bérlési idő  : {MaxBerlesIdotartamOra} óra / fő (összefüggő időtartam)");
            Console.WriteLine($"Min. létszám      : {MinVersenyzoLetszam} fő / menet");
            Console.WriteLine($"Max. létszám      : {MaxVersenyzoLetszam} fő / menet");
            Console.WriteLine();
        }
    }
}
