using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nagybeadando_teniszpalya
{
    public class Palya
    {
        public enum Boritas
        {
            Salak,
            Fuves,
            Muanyag
        }

        public int sorszam { get; set; }
        public Boritas boritas { get; set; }
        public bool fedett { get; set; }

        public Palya(int sorszam, Boritas boritas, bool fedett)
        {
            this.sorszam = sorszam;
            this.boritas = boritas;
            this.fedett = fedett;
        }

        public double getOradij()
        {
            double oradij = 0;
            switch (boritas)
            {
                case Boritas.Salak:
                    oradij = 3000; break;
                case Boritas.Fuves:
                    oradij = 5000; break;
                case Boritas.Muanyag:
                    oradij = 2000; break;
            }
            if (fedett)
            {
                oradij = oradij * 1.2;
            }
            return oradij;
        }

    }
}
