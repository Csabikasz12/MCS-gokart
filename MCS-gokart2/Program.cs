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
            Console.WriteLine("=== Gokart helyszín adatai ===");
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
    } // Gokartpalya vége


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
    } // Versenyzo vége


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

            string teljesSzoveg = File.ReadAllText(teljesUtvonal);
            string[] reszek = teljesSzoveg.Split(',');

            foreach (string resz in reszek)
            {
                string tisztitott = resz.Trim().Trim('\'', '"', ' ', '\r', '\n');
                if (!string.IsNullOrWhiteSpace(tisztitott))
                {
                    nevek.Add(tisztitott);
                }
            }

            if (nevek.Count == 0)
            {
                Console.WriteLine($"HIBA: A(z) '{fajlNev}' fájlból nem sikerült egyetlen nevet sem beolvasni!");
                Environment.Exit(1);
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
    } // NevGenerator vége


    public class Foglalas
    {
        public string VersenyzoAzonosito { get; set; }
        public DateTime Datum { get; set; }
        public int KezdoOra { get; set; }
    }


    public class Naptar
    {
        private List<Foglalas> foglalasok = new List<Foglalas>();
        private List<int> oraSavok = new List<int> { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
        private List<DateTime> napok = new List<DateTime>();

        private List<Versenyzo> elofoglaltVersenyzok = new List<Versenyzo>();

        public Naptar(List<string> vezeteknevek, List<string> keresztnevek, Random rnd)
        {
            DateTime ma = DateTime.Today;
            DateTime honapVege = new DateTime(ma.Year, ma.Month, DateTime.DaysInMonth(ma.Year, ma.Month));

            for (DateTime nap = ma; nap <= honapVege; nap = nap.AddDays(1))
            {
                napok.Add(nap);
            }

            ElofoglalasokGeneralasa(vezeteknevek, keresztnevek, rnd);
        }

        private void ElofoglalasokGeneralasa(List<string> vezeteknevek, List<string> keresztnevek, Random rnd)
        {
            int elofoglaltSavokSzama = rnd.Next(5, 15);

            for (int i = 0; i < elofoglaltSavokSzama; i++)
            {
                DateTime nap = napok[rnd.Next(napok.Count)];
                int ora = oraSavok[rnd.Next(oraSavok.Count)];

                int jelenlegiLetszam = FoglaltLetszam(nap, ora);
                if (jelenlegiLetszam >= Gokartpalya.MaxVersenyzoLetszam)
                {
                    continue;
                }

                int keztLetszam = rnd.Next(Gokartpalya.MinVersenyzoLetszam, Gokartpalya.MaxVersenyzoLetszam + 1);
                int hozzaadhato = Math.Min(keztLetszam, Gokartpalya.MaxVersenyzoLetszam - jelenlegiLetszam);

                for (int f = 0; f < hozzaadhato; f++)
                {
                    Versenyzo elofoglaltVersenyzo = NevGenerator.VeletlenVersenyzoGeneralasa(vezeteknevek, keresztnevek, rnd);
                    elofoglaltVersenyzok.Add(elofoglaltVersenyzo);

                    foglalasok.Add(new Foglalas
                    {
                        VersenyzoAzonosito = elofoglaltVersenyzo.VersenyzoAzonosito,
                        Datum = nap.Date,
                        KezdoOra = ora
                    });
                }
            }
        }

        public bool VanFoglalas(DateTime datum, int oraKezdet)
        {
            return foglalasok.Exists(f => f.Datum.Date == datum.Date && f.KezdoOra == oraKezdet);
        }

        public int FoglaltLetszam(DateTime datum, int oraKezdet)
        {
            return foglalasok.FindAll(f => f.Datum.Date == datum.Date && f.KezdoOra == oraKezdet).Count;
        }

        // ÚJ: visszaadja egy adott versenyző összes jelenlegi foglalását
        public List<Foglalas> VersenyzoFoglalasai(string azonosito)
        {
            return foglalasok.FindAll(f => f.VersenyzoAzonosito == azonosito);
        }

        public bool UjFoglalasErvenyes(DateTime datum, List<int> oraKezdoLista, out string hibaUzenet)
        {
            hibaUzenet = "";

            if (!napok.Exists(n => n.Date == datum.Date))
            {
                hibaUzenet = "A megadott dátum a mai nap és a hónap vége közötti tartományon kívül esik.";
                return false;
            }

            if (oraKezdoLista.Count < 1 || oraKezdoLista.Count > 2)
            {
                hibaUzenet = "Egy foglalás minimum 1, maximum 2 órára szólhat.";
                return false;
            }

            foreach (int ora in oraKezdoLista)
            {
                if (!oraSavok.Contains(ora))
                {
                    hibaUzenet = $"A(z) {ora} óra nem érvényes kezdő időpont (8 és 18 között lehet).";
                    return false;
                }
            }

            if (oraKezdoLista.Count == 2)
            {
                oraKezdoLista.Sort();
                if (oraKezdoLista[1] - oraKezdoLista[0] != 1)
                {
                    hibaUzenet = "A 2 órás foglalásnak összefüggőnek kell lennie (pl. 15 és 16).";
                    return false;
                }
            }

            foreach (int ora in oraKezdoLista)
            {
                int jelenlegiLetszam = FoglaltLetszam(datum, ora);
                if (jelenlegiLetszam >= Gokartpalya.MaxVersenyzoLetszam)
                {
                    hibaUzenet = $"A(z) {ora}-{ora + 1} sáv ({datum:yyyy.MM.dd}) már tele van ({Gokartpalya.MaxVersenyzoLetszam} fő).";
                    return false;
                }
            }

            return true;
        }

        public void FoglalasHozzaadasa(string azonosito, DateTime datum, List<int> oraKezdoLista)
        {
            foreach (int ora in oraKezdoLista)
            {
                foglalasok.Add(new Foglalas
                {
                    VersenyzoAzonosito = azonosito,
                    Datum = datum.Date,
                    KezdoOra = ora
                });
            }
        }

        public void VersenyzoFoglalasainakTorlese(string azonosito)
        {
            foglalasok.RemoveAll(f => f.VersenyzoAzonosito == azonosito);
        }

        public void IdoszalagKiirasa()
        {
            Console.WriteLine();
            Console.WriteLine("=== Időszalag (szám = aktuális/maximum létszám a sávban) ===");

            Console.Write("Dátum       ");
            foreach (int ora in oraSavok)
            {
                string cimke = $"{ora}-{ora + 1}";
                Console.Write(cimke.PadRight(6));
            }
            Console.WriteLine();

            foreach (DateTime nap in napok)
            {
                Console.Write($"{nap:yyyy.MM.dd} ");

                foreach (int ora in oraSavok)
                {
                    int letszam = FoglaltLetszam(nap, ora);
                    bool foglalt = letszam > 0;

                    Console.BackgroundColor = foglalt ? ConsoleColor.Red : ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;

                    string cellaSzoveg = $"{letszam}/{Gokartpalya.MaxVersenyzoLetszam}";
                    Console.Write(cellaSzoveg.PadRight(5));

                    Console.ResetColor();
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void FoglaltSavokListazasa()
        {
            Console.WriteLine();
            Console.WriteLine("=== Foglalt időpontok részletesen ===");

            bool voltFoglalas = false;

            foreach (DateTime nap in napok)
            {
                foreach (int ora in oraSavok)
                {
                    List<Foglalas> ittFoglaltak = foglalasok.FindAll(f => f.Datum.Date == nap.Date && f.KezdoOra == ora);

                    if (ittFoglaltak.Count > 0)
                    {
                        voltFoglalas = true;
                        Console.WriteLine($"{nap:yyyy.MM.dd} {ora}-{ora + 1}: {ittFoglaltak.Count} fő");

                        foreach (Foglalas f in ittFoglaltak)
                        {
                            Console.WriteLine($"    - {f.VersenyzoAzonosito}");
                        }
                    }
                }
            }

            if (!voltFoglalas)
            {
                Console.WriteLine("Jelenleg nincs egyetlen foglalás sem.");
            }

            Console.WriteLine();
        }
    } // Naptar vége


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("============================================================");
            Console.WriteLine(" Projekt neve  : Gokart időpontfoglaló - Egyéni kisprojekt");
            Console.WriteLine(" Készítette    : Mészáros Csaba (MCS)");
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
            }

            Console.WriteLine();
            Console.WriteLine($"Összesen {versenyzok.Count} versenyző került generálásra.");

            Naptar naptar = new Naptar(vezeteknevek, keresztnevek, rnd);
            naptar.IdoszalagKiirasa();

            bool kilepes = false;
            while (!kilepes)
            {
                Console.WriteLine("=== Menü ===");
                Console.WriteLine("1 - Versenyzők listázása");
                Console.WriteLine("2 - Foglalás beállítása / átállítása versenyzőhöz");
                Console.WriteLine("3 - Időszalag újra megjelenítése");
                Console.WriteLine("4 - Foglalt időpontok részletes listázása (kik foglaltak)");
                Console.WriteLine("0 - Kilépés");
                Console.Write("Választás: ");
                string valasztas = Console.ReadLine();

                switch (valasztas)
                {
                    case "1":
                        VersenyzokListazasa(versenyzok);
                        break;

                    case "2":
                        FoglalasKezeles(versenyzok, naptar);
                        break;

                    case "3":
                        naptar.IdoszalagKiirasa();
                        break;

                    case "4":
                        naptar.FoglaltSavokListazasa();
                        break;

                    case "0":
                        kilepes = true;
                        break;

                    default:
                        Console.WriteLine("Érvénytelen választás, próbáld újra.");
                        break;
                }
            }

            Console.WriteLine("Nyomj meg egy billentyűt a kilépéshez...");
            Console.ReadKey();
        }

        static void VersenyzokListazasa(List<Versenyzo> versenyzok)
        {
            Console.WriteLine();
            Console.WriteLine("=== Versenyzők listája ===");
            foreach (Versenyzo v in versenyzok)
            {
                Console.WriteLine($"{v.VersenyzoAzonosito.PadRight(30)} | {v.Vezeteknev} {v.Keresztnev}");
            }
            Console.WriteLine();
        }

        static Versenyzo VersenyzoKivalasztasa(List<Versenyzo> versenyzok)
        {
            VersenyzokListazasa(versenyzok);
            Console.Write("Add meg a versenyző-azonosítót: ");
            string azonosito = Console.ReadLine();

            Versenyzo talalt = versenyzok.Find(v => v.VersenyzoAzonosito == azonosito);

            if (talalt == null)
            {
                Console.WriteLine("Nem található ilyen azonosítójú versenyző.");
            }

            return talalt;
        }

        // Foglalás beállítása vagy átállítása: most már megmutatja a meglévő foglalásokat,
        // és rákérdez, hogy tényleg módosítani szeretnéd-e, mielőtt bármit bekérne
        static void FoglalasKezeles(List<Versenyzo> versenyzok, Naptar naptar)
        {
            Versenyzo kivalasztott = VersenyzoKivalasztasa(versenyzok);
            if (kivalasztott == null)
            {
                return;
            }

            List<Foglalas> meglevoFoglalasok = naptar.VersenyzoFoglalasai(kivalasztott.VersenyzoAzonosito);

            if (meglevoFoglalasok.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"A(z) {kivalasztott.VersenyzoAzonosito} azonosítónak jelenleg van foglalása:");
                foreach (Foglalas f in meglevoFoglalasok)
                {
                    Console.WriteLine($"    - {f.Datum:yyyy.MM.dd} {f.KezdoOra}-{f.KezdoOra + 1}");
                }

                Console.Write("Szeretnéd módosítani ezt a foglalást? (i/n): ");
                string valasz = Console.ReadLine();

                if (valasz == null || valasz.Trim().ToLower() != "i")
                {
                    Console.WriteLine("A foglalás nem változott.");
                    return;
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"A(z) {kivalasztott.VersenyzoAzonosito} azonosítónak jelenleg nincs foglalása.");
            }

            Console.Write("Add meg a dátumot (éééé.hh.nn formátumban, pl. 2026.09.26): ");
            string datumSzoveg = Console.ReadLine();

            DateTime datum;
            bool datumOk = DateTime.TryParseExact(
                datumSzoveg.Trim(),
                "yyyy.MM.dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out datum);

            if (!datumOk)
            {
                Console.WriteLine("Érvénytelen dátumformátum.");
                return;
            }

            Console.Write("Add meg a kezdő órát (8 és 18 között, pl. 8): ");
            string kezdoOraSzoveg = Console.ReadLine();

            int kezdoOra;
            if (!int.TryParse(kezdoOraSzoveg.Trim(), out kezdoOra))
            {
                Console.WriteLine("Érvénytelen óra.");
                return;
            }

            Console.Write("Hány órára szól a foglalás (1 vagy 2): ");
            string idotartamSzoveg = Console.ReadLine();

            int idotartam;
            if (!int.TryParse(idotartamSzoveg.Trim(), out idotartam) || (idotartam != 1 && idotartam != 2))
            {
                Console.WriteLine("Érvénytelen időtartam, csak 1 vagy 2 lehet.");
                return;
            }

            List<int> oraKezdoLista = new List<int> { kezdoOra };
            if (idotartam == 2)
            {
                oraKezdoLista.Add(kezdoOra + 1);
            }

            naptar.VersenyzoFoglalasainakTorlese(kivalasztott.VersenyzoAzonosito);

            string hibaUzenet;
            if (!naptar.UjFoglalasErvenyes(datum, oraKezdoLista, out hibaUzenet))
            {
                Console.WriteLine($"Érvénytelen foglalás: {hibaUzenet}");
                return;
            }

            naptar.FoglalasHozzaadasa(kivalasztott.VersenyzoAzonosito, datum, oraKezdoLista);

            Console.WriteLine("Foglalás sikeresen beállítva / átállítva.");
            naptar.IdoszalagKiirasa();
        }
    } // Program vége

} // namespace vége