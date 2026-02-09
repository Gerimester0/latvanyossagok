using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // A fájl neve most már orszagok.txt
            string filePath = "C:\\Users\\PappG\\source\\repos\\ConsoleApp1\\ConsoleApp1\\orszagok.txt";
            List<Helyseg> helysegek = AdatokBeolvasasa(filePath);

            if (helysegek == null || helysegek.Count == 0)
            {
                Console.WriteLine("Nem sikerült adatokat beolvasni. A program leáll.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Földrajzi adatokból származó maximumok:");
            Console.WriteLine("==========================================");

            // Maximumok meghatározása és kiírása
            MaximumokKiirasa(helysegek);

            Console.WriteLine("\nFeldolgozás kész. Nyomj meg egy gombot a kilépéshez...");
            Console.ReadKey();
        }

        // Adatok beolvasása a fájlból (Ez a metódus csak egyszer szerepelhet!)
        private static List<Helyseg> AdatokBeolvasasa(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Hiba: A(z) '{filePath}' fájl nem található.");
                    return null;
                }

                string[] sorok = File.ReadAllLines(filePath);
                List<Helyseg> adatok = new List<Helyseg>();

                foreach (string sor in sorok)
                {
                    string tisztitottSor = sor.Trim();
                    string[] elemek = tisztitottSor.Split(';');

                    if (elemek.Length == 6 &&
                        int.TryParse(elemek[3].Trim(), out int epitesEve) &&
                        double.TryParse(elemek[4].Trim(), out double szelesseg) &&
                        double.TryParse(elemek[5].Trim(), out double hosszusag))
                    {
                        adatok.Add(new Helyseg(
                            elemek[0].Trim(),
                            elemek[1].Trim(),
                            elemek[2].Trim(),
                            epitesEve,
                            szelesseg,
                            hosszusag));
                    }
                    else
                    {
                        Console.WriteLine($"Figyelmeztetés: A következő sor feldolgozása sikertelen: '{tisztitottSor}'");
                    }
                }

                return adatok;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Váratlan hiba történt a beolvasás során: {ex.Message}");
                return null;
            }
        }

        // A maximumok kiírása
        static void MaximumokKiirasa(List<Helyseg> helysegek)
        {
            // 1. Legrégebbi építésű
            var legrégebbi = helysegek.OrderBy(h => h.EpitesEve).FirstOrDefault();
            Console.WriteLine($"Legrégebbi építésű: {legrégebbi?.Nevezetessege} ({legrégebbi?.EpitesEve})");

            // 2. Legújabb építésű
            var legújabb = helysegek.OrderByDescending(h => h.EpitesEve).FirstOrDefault();
            Console.WriteLine($"Legújabb építésű: {legújabb?.Nevezetessege} ({legújabb?.EpitesEve})");

            // 3. Legtávolabb északon (legnagyobb szélességi fok)
            var legészakibb = helysegek.OrderByDescending(h => h.Szellesseg).FirstOrDefault();
            Console.WriteLine($"Legészakibb pont: {legészakibb?.Nevezetessege} ({legészakibb?.Varos}) [Szélesség: {legészakibb?.Szellesseg}]");

            // 4. Legtávolabb délen (legkisebb szélességi fok)
            var legdélibb = helysegek.OrderBy(h => h.Szellesseg).FirstOrDefault();
            Console.WriteLine($"Legdélibb pont: {legdélibb?.Nevezetessege} ({legdélibb?.Varos}) [Szélesség: {legdélibb?.Szellesseg}]");

            // 5. Legtávolabb keleten (legnagyobb hosszúsági fok)
            var legkeletibb = helysegek.OrderByDescending(h => h.Hosszusag).FirstOrDefault();
            Console.WriteLine($"Legkeletibb pont: {legkeletibb?.Nevezetessege} ({legkeletibb?.Varos}) [Hosszúság: {legkeletibb?.Hosszusag}]");

            // 6. Legtávolabb nyugaton (legkisebb hosszúsági fok)
            var legnyugatibb = helysegek.OrderBy(h => h.Hosszusag).FirstOrDefault();
            Console.WriteLine($"Legnyugatibb pont: {legnyugatibb?.Nevezetessege} ({legnyugatibb?.Varos}) [Hosszúság: {legnyugatibb?.Hosszusag}]");
        }
    }
}