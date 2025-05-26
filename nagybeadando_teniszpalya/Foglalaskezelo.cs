using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nagybeadando_teniszpalya
{
    public class Foglalaskezelo
    {
        public Foglalas foglalas { get; private set; }
        public Klub klub; //referencia a Klub osztályra

        public Foglalaskezelo(Klub klub)
        {
            this.klub = klub ?? throw new ArgumentNullException(nameof(klub));
        }

        //visszaadja az összes szabad pályát az adott időre és borítás fajtára
        public List<Palya> SzabadPalyaKeres(DateTime datum, int ora, Palya.Boritas boritas)
        {
            return klub.palyak
                .Where(p => p.boritas == boritas &&
                           !klub.foglalasok.Any(f =>
                               f.palya == p &&
                               f.datum.Date == datum.Date &&
                               f.ora == ora &&
                               f.allapot != Foglalas.Allapot.Visszamondott))
                .ToList();
        }

        public bool Foglal(Tag tag, Palya palya, DateTime datum, int ora) //bool mert returnoljuk hogy sikeres volt-e vagy nem
        {
            if (!SzabadPalyaKeres(datum, ora, palya.boritas).Contains(palya))
                return false;

            var ujFoglalas = new Foglalas(tag, palya, datum, ora, Foglalas.Allapot.Foglalt);

            klub.foglalasok.Add(ujFoglalas);
            return true;
        }

        public bool Lemond(Foglalas foglalas)
        {
            var torlendo = klub.foglalasok.FirstOrDefault(f =>
            f.palya == foglalas.palya &&
            f.datum == foglalas.datum &&
            f.ora == foglalas.ora &&
            f.allapot == Foglalas.Allapot.Foglalt);

            if (torlendo == null) return false;

            torlendo.allapot = Foglalas.Allapot.Visszamondott;
            klub.foglalasok.Remove(torlendo);
            return true;
        }

        //adott tag adott dátumra való összes foglalása (pálya és óra)
        public List<Foglalas> TagNapiFoglalasai(Tag tag, DateTime nap)
        {
            List<Foglalas> napiFoglalasok = new List<Foglalas>();

            foreach (var f in klub.foglalasok)
            {
                if (f.tag == tag && f.datum == nap)
                {
                    napiFoglalasok.Add(f);
                }
            }
            if (napiFoglalasok == null)
            {
                Console.WriteLine("nem volt ennek a tagnak foglalása ezen a napon");
            }
            return napiFoglalasok;
        }

        //adott tag adott napra való összes díja
        public int tagHasznalatiDij(Tag tag, DateTime nap)
        {
            int osszeg = 0;
            foreach (var f in klub.foglalasok)
            {
                if (f.tag == tag && f.datum == nap
                    && (f.allapot == Foglalas.Allapot.Foglalt || f.allapot == Foglalas.Allapot.Teljesitett))
                {
                    double basePrice = f.palya.getOradij();
                    osszeg += (int)tag.ApplyDiscount(basePrice);
                }
            }
            return osszeg;
        }

        public int klubBevetel(DateTime kezdoDatum, DateTime vegDatum)
        {
            int bevetel = 0;
            foreach (var f in klub.foglalasok)
            {
                if (f.datum >= kezdoDatum && f.datum <= vegDatum
                   && f.allapot == Foglalas.Allapot.Teljesitett)
                {
                    double basePrice = f.palya.getOradij();
                    bevetel += (int)f.tag.ApplyDiscount(basePrice);
                }
            }
            return bevetel;
        }

    }
}
