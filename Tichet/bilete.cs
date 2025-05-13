using System;
using System.IO;
using LocParcare;
using Masina;

namespace Tichet
{
    public enum Valuta { RON, EUR, USD }
    public enum ModPlata { Cash, Card }

    public class Bilete
    {
        public const string FisierPlati = "plati.txt";
        private const double pretPeSecundaNormal = 0.00138;
        private const double pretPeSecundaVIP = 0.00416;

        public int NumarLoc { get; }
        private DetaliiMasina masina;
        private Loc loc;

        public Bilete(DetaliiMasina masina, Loc loc)
        {
            this.masina = masina;
            this.loc = loc;
            this.NumarLoc = loc.NumarLoc;
        }

        public double CalculeazaCost(Valuta valuta)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday ||
                DateTime.Now.Hour >= 21 ||
                DateTime.Now.Hour < 7)
            {
                return 0;
            }

            TimeSpan durata = DateTime.Now - loc.DataOcupare;
            double costRON = durata.TotalSeconds * (loc.VIP ? pretPeSecundaVIP : pretPeSecundaNormal);

            switch (valuta)
            {
                case Valuta.EUR: return costRON * 0.2;
                case Valuta.USD: return costRON * 0.22;
                default: return costRON;
            }
        }

        public void EfectueazaPlata(ModPlata modPlata, Valuta valuta, double cost)
        {
            string mesajPlata = $"{DateTime.Now};{masina.NumarInmatriculare};{modPlata};{valuta};{cost:F2};{(DateTime.Now - loc.DataOcupare).TotalSeconds:F0}s";

            using (StreamWriter sw = new StreamWriter(FisierPlati, true))
            {
                sw.WriteLine(mesajPlata);
            }

            Console.WriteLine($"\nPlata efectuata:");
            Console.WriteLine($"- Suma: {cost:F2} {valuta}");
            Console.WriteLine($"- Metoda: {modPlata}");
            Console.WriteLine($"- Durata: {(DateTime.Now - loc.DataOcupare).TotalSeconds:F0} secunde");
            Console.WriteLine($"- Pret/sec: {(loc.VIP ? pretPeSecundaVIP : pretPeSecundaNormal)} RON");
        }

        public override string ToString()
        {
            return $"Bilet parcare #{NumarLoc} {(loc.VIP ? "(VIP)" : "")}\n" +
                   $"Masina: {masina.NumarInmatriculare}\n" +
                   $"Proprietar: {masina.Proprietar}\n" +
                   $"Marca: {masina.Marca}\n" +
                   $"Culoare: {masina.Culoare}\n" +
                   $"Data intrare: {loc.DataOcupare}\n" +
                   $"Tip parcare: {(loc.VIP ? "VIP" : "Standard")}\n" +
                   $"Pret curent: {CalculeazaCost(Valuta.RON):F2} RON";
        }
    }
}
