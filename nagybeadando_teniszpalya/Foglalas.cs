using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nagybeadando_teniszpalya
{
    public class Foglalas
    {
        public enum Allapot
        {
            Foglalt,
            Teljesitett,
            Visszamondott
        }

        public Tag tag { get; set; }
        public Palya palya { get; set; }
        public DateTime datum { get; set; }
        public int ora { get; set; }
        public Allapot allapot { get; set; }

        public Foglalas(Tag tag, Palya palya, DateTime datum, int ora, Allapot allapot)
        {
            this.tag = tag ?? throw new ArgumentNullException(nameof(tag));
            this.palya = palya ?? throw new ArgumentNullException(nameof(palya));
            this.datum = datum;
            this.ora = ora;
            this.allapot = allapot;
        }

    }
}
