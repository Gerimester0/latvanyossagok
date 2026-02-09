using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization; // Hozzáadva a CultureInfo miatt

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // A fájl neve most már orszagok.txt
            string filePath = "orszagok.txt"; // Egyszerűsítettem az elérési utat, feltételezve, hogy a fájl a bin/Debug mappában van.
                                              // Ha nem, írd vissza a teljes elérési utat.
            List<Helyseg> helysegek = AdatokBeolvasasa(filePath);

            if (helysegek == null || helysegek.Count == 0)
            {
                Console.WriteLine("Nem sikerült adatokat beolvasni. A program leáll.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Adatok sikeresen beolvasva. Összesen {0} helység.", helysegek.Count);

            bool kilep = false;
            while (!kilep)
            {
                Console.WriteLine("\nVálassz egy feladatot:");
                Console.WriteLine("1. Listázás országonként");
                Console.WriteLine("2. Keresés országnév szerint");
                Console.WriteLine("3. Legrégebbi építmények listázása (top 5)");
                Console.WriteLine("4. Éghajlati övezetek szerinti csoportosítás");
                Console.WriteLine("5. Kilépés");
                Console.Write("Kérem a választott feladat számát: ");

                string valasz = Console.ReadLine();
                int valaszSzam;

                if (int.TryParse(valasz, out valaszSzam))
                {
                    Console.WriteLine($"DEBUG: Felhasználó a(z) {valaszSzam}. feladatot választotta.");
                    switch (valaszSzam)
                    {
                        case 1:
                            Console.WriteLine("DEBUG: 1. feladat indítása - Listázás országonként");
                            ListazasOrszagokSzerint(helysegek);
                            break;
                        case 2:
                            Console.WriteLine("DEBUG: 2. feladat indítása - Keresés országnév szerint");
                            KeresesOrszagNevSzerint(helysegek);
                            break;
                        case 3:
                            Console.WriteLine("DEBUG: 3. feladat indítása - Legrégebbi építmények listázása");
                            LegregebbiEpuletekListazasa(helysegek);
                            break;
                        case 4:
                            Console.WriteLine("DEBUG: 4. feladat indítása - Éghajlati övezetek szerinti csoportosítás");
                            EghajlatiOvezetekSzerintiCsoportositas(helysegek);
                            break;
                        case 5:
                            Console.WriteLine("DEBUG: 5. feladat indítása - Kilépés");
                            kilep = true;
                            Console.WriteLine("Kilépés a programból...");
                            break;
                        default:
                            Console.WriteLine(\$"DEBUG: Érvénytelen választás: {valaszSzam}");
                            Console.WriteLine("Érvénytelen menüpont! Kérem, 1 és 5 között adjon meg számot.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"DEBUG: Érvénytelen bemenet: '{valasz}'");
                    Console.WriteLine("Érvénytelen bemenet! Kérem, számot adjon meg.");
                }

                if (!kilep)
                {
                    Console.WriteLine("\nNyomjon meg egy gombot a folytatáshoz...");
                    Console.ReadKey();
                    Console.Clear(); // Törli a képernyőt a következő menühöz
                }
            }

            Console.WriteLine("\nFeldolgozás kész. Nyomj meg egy gombot a kilépéshez...");
            Console.ReadKey();
        }

        // Adatok beolvasása a fájlból (Ez a metódus csak egyszer szerepelhet!)
        private static List<Helyseg> AdatokBeolvasasa(string filePath)
        {
            try
            {
                Console.WriteLine($"DEBUG: Adatok beolvasása megkezdése a következő fájlból: {filePath}");
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Hiba: A(z) '{filePath}' fájl nem található.");
                    Console.WriteLine("DEBUG: A fájl nem létezik. Kérlek, helyezd az 'orszagok.txt' fájlt a program futtatható mappájába (pl. bin/Debug/netX.X).");
                    return null;
                }

                string[] sorok = File.ReadAllLines(filePath);
                Console.WriteLine($"DEBUG: {sorok.Length} sor beolvasva a fájlból.");
                List<Helyseg> adatok = new List<Helyseg>();

                foreach (string sor in sorok)
                {
                    string tisztitottSor = sor.Trim();
                    if (string.IsNullOrEmpty(tisztitottSor)) continue; // Kihagyja az üres sorokat

                    string[] elemek = tisztitottSor.Split(';');

                    if (elemek.Length == 6)
                    {
                        // Építés évének ellenőrzése
                        if (!int.TryParse(elemek[3].Trim(), out int epitesEve))
                        {
                            Console.WriteLine($"Figyelmeztetés: Érvénytelen építés éve a következő sorban: '{tisztitottSor}'");
                            continue;
                        }

                        // Szélesség és hosszúság ellenőrzése - InvariantCulture használatával
                        if (!double.TryParse(elemek[4].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double szelesseg))
                        {
                            Console.WriteLine($"Figyelmeztetés: Érvénytelen szélességi koordináta a következő sorban: '{tisztitottSor}'");
                            continue;
                        }

                        if (!double.TryParse(elemek[5].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double hosszusag))
                        {
                            Console.WriteLine($"Figyelmeztetés: Érvénytelen hosszúsági koordináta a következő sorban: '{tisztitottSor}'");
                            continue;
                        }

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
                        Console.WriteLine($"Figyelmeztetés: A következő sor feldolgozása sikertelen (nem 6 elem, hanem {elemek.Length}): '{tisztitottSor}'");
                    }
                }

                Console.WriteLine($"DEBUG: Sikeresen beolvasva {adatok.Count} helység.");
                return adatok;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Váratlan hiba történt a beolvasás során: {ex.Message}");
                return null;
            }
        }

        // 1. feladat: Listázás országonként
        static void ListazasOrszagokSzerint(List<Helyseg> helysegek)
        {
            Console.WriteLine("\n1. Feladat - Listázás országonként:");
            Console.WriteLine("==================================");

            var csoportositas = helysegek.GroupBy(h => h.Orszag)
                                       .OrderBy(g => g.Key);

            foreach (var csoport in csoportositas)
            {
                Console.WriteLine($"\nOrszág: {csoport.Key} ({csoport.Count()} helység):");
                foreach (var helyseg in csoport)
                {
                    Console.WriteLine($"  - {helyseg.Varos}: {helyseg.Nevezetessege} ({helyseg.EpitesEve})");
                }
            }
        }

        // 2. feladat: Keresés országnév szerint
        static void KeresesOrszagNevSzerint(List<Helyseg> helysegek)
        {
            Console.WriteLine("\n2. Feladat - Keresés országnév szerint:");
            Console.WriteLine("======================================");
            Console.Write("Kérem az ország nevét (részben is kereshető): ");
            string keresettOrszag = Console.ReadLine().Trim().ToLower();
            Console.WriteLine($"DEBUG: Keresett országnév: '{keresettOrszag}'");

            var talalatok = helysegek.Where(h => h.Orszag.ToLower().Contains(keresettOrszag)).ToList();

        }
    } 
}