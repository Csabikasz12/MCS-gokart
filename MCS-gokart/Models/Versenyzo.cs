using System;
using System.Globalization;
using System.Text;

namespace MCS_gokart.Models
{
    /// <summary>
    /// Egy gokart versenyzőt reprezentáló osztály.
    /// A versenyző-azonosító és az email cím a konstruktorban,
    /// automatikusan generálódik a többi adatból.
    /// </summary>
    public class Versenyzo
    {
        public string Vezeteknev { get; private set; }
        public string Keresztnev { get; private set; }
        public DateTime SzuletesiIdo { get; private set; }
        public string VersenyzoAzonosito { get; private set; }
        public string Email { get; private set; }

        /// <summary>
        /// Kiszámított tulajdonság: elmúlt-e már 18 éves a versenyző
        /// az aktuális dátumhoz (DateTime.Today) képest.
        /// </summary>
        public bool Elmult18Eves
        {
            get
            {
                DateTime ma = DateTime.Today;
                int eletkor = ma.Year - SzuletesiIdo.Year;

                // Ha az idei "évforduló" még nem volt meg, egy évet levonunk
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

        /// <summary>
        /// Versenyző-azonosító összeállítása:
        /// GO- + teljes név (ékezet nélkül) + - + születési dátum (yyyyMMdd)
        /// pl.: GO-KovacsDenes-19741204
        /// </summary>
        private string AzonositoGeneralasa()
        {
            string teljesNevEkezetNelkul = EkezetEltavolitasa(Vezeteknev + Keresztnev);
            string szuletesiDatumEgyben = SzuletesiIdo.ToString("yyyyMMdd");
            return $"GO-{teljesNevEkezetNelkul}-{szuletesiDatumEgyben}";
        }

        /// <summary>
        /// Email cím összeállítása:
        /// vezeteknev.keresztnev@gmail.com (ékezetek nélkül, kisbetűvel)
        /// </summary>
        private string EmailGeneralasa()
        {
            string vezeteknevTiszta = EkezetEltavolitasa(Vezeteknev).ToLower();
            string keresztnevTiszta = EkezetEltavolitasa(Keresztnev).ToLower();
            return $"{vezeteknevTiszta}.{keresztnevTiszta}@gmail.com";
        }

        /// <summary>
        /// Magyar ékezetes karakterek (á, é, í, ó, ö, ő, ú, ü, ű, stb.)
        /// eltávolítása egy szövegből.
        /// </summary>
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

            // Az "ő" és "ű" betűket a .NET Normalize metódusa nem mindig
            // bontja fel helyesen, ezért ezeket külön, biztosan lecseréljük.
            return eredmeny.ToString()
                .Normalize(NormalizationForm.FormC)
                .Replace('ő', 'o').Replace('Ő', 'O')
                .Replace('ű', 'u').Replace('Ű', 'U');
        }

        /// <summary>
        /// A versenyző adatainak kiírása a konzolra.
        /// </summary>
        public void AdatokKiirasa()
        {
            Console.WriteLine($"Név              : {Vezeteknev} {Keresztnev}");
            Console.WriteLine($"Születési idő    : {SzuletesiIdo:yyyy.MM.dd}");
            Console.WriteLine($"Elmúlt 18 éves   : {Elmult18Eves}");
            Console.WriteLine($"Versenyző-azon.  : {VersenyzoAzonosito}");
            Console.WriteLine($"Email cím        : {Email}");
        }
    }
}
