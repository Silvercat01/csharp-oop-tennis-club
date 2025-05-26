using TextFile;
using System;
using System.Collections.Generic;

namespace nagybeadando_teniszpalya
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Inicializálás
            var tagok = new List<Tag>();
            var palyak = new List<Palya>();
            var foglalasok = new List<Foglalas>();

            // 2. Klub létrehozása ÜRES foglaláskezelővel
            var klub = new Klub(tagok, palyak, foglalasok, null);

            // 3. Foglaláskezelő létrehozása a KLUBBAL
            klub.foglalaskezelo = new Foglalaskezelo(klub); // Itt már van klub referenciája

            // 4. Példaadatok betöltése
            var strategies = new Dictionary<Tag.Kedvezmeny, IPricingStrategy>
            {
                { Tag.Kedvezmeny.Igazolt, new IgazoltPricingStrategy() },
                { Tag.Kedvezmeny.Diak, new DiakPricingStrategy() },
                { Tag.Kedvezmeny.Nyugdijas, new NyugdijasPricingStrategy() },
                { Tag.Kedvezmeny.Altalanos, new AltalanosPricingStrategy() }
            };

            string fajlnev = "Input.txt";
            LoadInitialData(fajlnev, klub, strategies);

            // 5. Példaműveletek futtatása
            RunExampleOperations(klub);
        }

        static void LoadInitialData(string filename, Klub klub, Dictionary<Tag.Kedvezmeny, IPricingStrategy> strategies)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine($"A {filename} fájl nem található!");
                return;
            }

            var reader = new TextFileReader(filename);
            string line;
            int section = 0; // 0=tagok, 1=pályák, 2=foglalások

            while ((line = reader.ReadLine()) != null)
            {
                // Üres sor választja a szakaszokat
                if (string.IsNullOrWhiteSpace(line))
                {
                    section++;
                    continue;
                }

                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) continue;

                try
                {
                    switch (section)
                    {
                        case 0: // Tagok
                            if (parts.Length >= 2)
                            {
                                if (Enum.TryParse(parts[1], out Tag.Kedvezmeny kedvezmeny) &&
                                    strategies.ContainsKey(kedvezmeny))
                                {
                                    klub.beleptet(new Tag(parts[0], kedvezmeny, strategies[kedvezmeny]));
                                }
                            }
                            break;

                        case 1: // Pályák
                            if (parts.Length >= 3)
                            {
                                if (int.TryParse(parts[0], out int sorszam) &&
                                    Enum.TryParse(parts[1], out Palya.Boritas boritas) &&
                                    int.TryParse(parts[2], out int fedett))
                                {
                                    klub.ujPalya(new Palya(sorszam, boritas, fedett == 1));
                                }
                            }
                            break;

                        case 2: // Foglalások
                            if (parts.Length >= 5)
                            {
                                var tag = klub.tagok.FirstOrDefault(t => t.nev == parts[0]);
                                var palya = klub.palyak.FirstOrDefault(p => p.sorszam == int.Parse(parts[1]));

                                if (tag != null && palya != null &&
                                    DateTime.TryParse(parts[2] + " " + parts[3], out DateTime datum) &&
                                    Enum.TryParse(parts[4], out Foglalas.Allapot allapot))
                                {
                                    klub.foglalasok.Add(new Foglalas(tag, palya, datum.Date, int.Parse(parts[3]), allapot));
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hiba a sor feldolgozásában: '{line}'. Hiba: {ex.Message}");
                }
            }
        }

        static void RunExampleOperations(Klub klub)
        {
            try
            {
                Console.WriteLine("Példa műveletek:");

                // 1. Tag létrehozása
                var tag = new Tag("Pista", Tag.Kedvezmeny.Altalanos, new AltalanosPricingStrategy());
                klub.beleptet(tag);
                Console.WriteLine($"Új tag hozzáadva: {tag.nev}");

                // 2. Pálya kiválasztása
                if (!klub.palyak.Any())
                {
                    Console.WriteLine("Nincsenek pályák a klubban!");
                    return;
                }
                var palya = klub.palyak.First();
                Console.WriteLine($"Pálya óradíja: {palya.getOradij()} Ft");

                // 3. Foglalás
                var holnap = DateTime.Now.AddDays(1);
                bool foglalasSikeres = klub.foglalaskezelo.Foglal(tag, palya, holnap, 14);
                Console.WriteLine($"Foglalás eredménye: {(foglalasSikeres ? "sikeres" : "sikertelen")}");

                // 4. Állapot beállítása
                var foglalas = klub.foglalasok.Last();
                foglalas.allapot = Foglalas.Allapot.Teljesitett;

                // 5. Díj kiszámolása
                int dij = klub.foglalaskezelo.tagHasznalatiDij(tag, holnap);
                Console.WriteLine($"A tag holnapi díja: {dij} Ft");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HIBA: {ex.Message}");
            }
        }
    }
}