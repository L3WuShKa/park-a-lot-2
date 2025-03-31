using System;
using System.IO;
using Masina;
using LocParcare;

namespace Tichet
{
    public enum Valuta { RON, EUR, USD }
    public enum ModPlata { Cash, Card }

    public class Bilete
    {
        public const string FisierPlati = "plati.txt";
        private const double pretPeSecundaNormal = 7.0; // 7 lei/secunda  //doar experimental
        private const double pretPeSecundaVIP = 14.0;   // 14 lei/secunda

        public int NumarLoc { get; }
        private DetaliiMasina masina;
        private Loc loc;
        private DateTime dataIntrare;

        public Bilete(DetaliiMasina masina, Loc loc, DateTime dataIntrare)
        {
            this.masina = masina;
            this.loc = loc;
            this.NumarLoc = loc.NumarLoc;
            this.dataIntrare = dataIntrare;
        }

        public double CalculeazaCost(Valuta valuta)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday ||
                DateTime.Now.Hour >= 21 ||
                DateTime.Now.Hour < 7)
            {
                return 0;
            }

            TimeSpan durata = DateTime.Now - dataIntrare;
            double costRON = durata.TotalSeconds * (loc.VIP ? pretPeSecundaVIP : pretPeSecundaNormal );

            switch (valuta)
            {
                case Valuta.EUR: return costRON * 0.2;  // 1 euro=5 RON
                case Valuta.USD: return costRON * 0.22; // 1Usd= 4.5RON
                default: return costRON;
            }
        }

        public void EfectueazaPlata(ModPlata modPlata, Valuta valuta, double cost)
        {
            string mesajPlata = $"{DateTime.Now};{masina.NumarInmatriculare};{modPlata};{valuta};{cost:F2};{(DateTime.Now - dataIntrare).TotalSeconds:F0}s";
                  //scriu in fisier in format cu ";" intre elementele care o sa le extrag cu split dupa
            using (StreamWriter sw = new StreamWriter(FisierPlati, true))
            {
                sw.WriteLine(mesajPlata);
            }

            Console.WriteLine($"\nPlata efectuata:");
            Console.WriteLine($"- Suma: {cost:F2} {valuta}");
            Console.WriteLine($"- Metoda: {modPlata}");
            Console.WriteLine($"- Durata: {(DateTime.Now - dataIntrare).TotalSeconds:F0} secunde");
            Console.WriteLine($"- Pret/sec: {(loc.VIP ? pretPeSecundaVIP : pretPeSecundaNormal)} RON");
        }

        public override string ToString()
        { // folosesc in meniu ca sa afisez detaliile biletului
            return $"Bilet parcare #{NumarLoc} {(loc.VIP ? "(VIP)" : "")}\n" +
                   $"Masina: {masina.NumarInmatriculare}\n" +
                   $"Proprietar: {masina.Proprietar}\n" +
                   $"Marca: {masina.Marca}\n" +
                   $"Culoare: {masina.Culoare}\n" +
                   $"Data intrare: {dataIntrare}\n" +
                   $"Tip parcare: {(loc.VIP ? "VIP" : "Standard")}\n" +
                   $"Pret curent: {CalculeazaCost(Valuta.RON):F2} RON"; // valuta.ron de la enum
        }
    }
}