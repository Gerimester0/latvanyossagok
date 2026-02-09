// Helysegek.cs (vagy Helyseg.cs)

using System;

// A namespace-nek egyeznie kell a Program.cs-ben lévővel
namespace ConsoleApp1
{
    public class Helyseg
    {
        public string Orszag { get; set; }
        public string Varos { get; set; }
        public string Nevezetessege { get; set; }
        public int EpitesEve { get; set; }
        public double Szellesseg { get; set; }
        public double Hosszusag { get; set; }

        public Helyseg(string orszag, string varos, string nevezetessege, int epitesEve, double szellesseg, double hosszusag)
        {
            Orszag = orszag;
            Varos = varos;
            Nevezetessege = nevezetessege;
            EpitesEve = epitesEve;
            Szellesseg = szellesseg;
            Hosszusag = hosszusag;
        }
    }
}