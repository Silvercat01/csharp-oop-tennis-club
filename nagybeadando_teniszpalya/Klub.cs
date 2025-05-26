using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nagybeadando_teniszpalya
{
    public class Klub
    {
        public List<Tag> tagok { get; set; }
        public List<Palya> palyak { get; set; }
        public List<Foglalas> foglalasok { get; set; }
        public Foglalaskezelo foglalaskezelo { get; set; }

        public Klub(List<Tag> tagok, List<Palya> palyak, List<Foglalas> foglalasok, Foglalaskezelo foglalaskezelo)
        {
            this.palyak = palyak ?? throw new ArgumentNullException(nameof(palyak));
            this.tagok = tagok ?? throw new ArgumentNullException(nameof(tagok));
            this.foglalasok = foglalasok ?? throw new ArgumentNullException(nameof(foglalasok));
            this.foglalaskezelo = foglalaskezelo;
        }

        public void beleptet(Tag tag)
        {
            if (!tagok.Any(t => t.nev == tag.nev))
                tagok.Add(tag);
        }

        public void kileptet(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            if (!tagok.Contains(tag))
            {
                throw new InvalidOperationException("A megadott tag nem található a klub tagjai között.");
            }
            tagok.Remove(tag);
        }

        public void ujPalya(Palya palya)
        {
            if (palya == null)
                throw new ArgumentNullException(nameof(palya));

            if (palyak.Any(p => p.sorszam == palya.sorszam))
                throw new InvalidOperationException("Már létezik ilyen sorszámú pálya.");

            palyak.Add(palya);
        }

        public void felszamolPalya(Palya palya)
        {
            if (palya == null)
                throw new ArgumentNullException(nameof(palya));

            if (!palyak.Contains(palya))
            {
                throw new InvalidOperationException("A megadott pálya nem található a klub pályái között.");
            }
            palyak.Remove(palya);
        }

    }
}
