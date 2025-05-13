using System;
using System.IO;
using LocParcare;
using Masina;
using Securitate;
using Tichet;
using System.Linq;
using System.Collections.Generic;

namespace ParcareMare
{
    public class toata_parcarea
    {
        private Loc[] locuri;
        private DetaliiMasina[] masiniParcate;
        private Bilete[] bilete;
        private const string fisierMasini = "masini.txt";

        public toata_parcarea(int numarLocuri)
        {
            locuri = new Loc[numarLocuri];
            masiniParcate = new DetaliiMasina[numarLocuri];
            bilete = new Bilete[numarLocuri];

            for (int i = 0; i < numarLocuri; i++)
            {
                locuri[i] = new Loc(i + 1);
            }

            IncarcaMasiniDinFisier();
        }

        private void IncarcaMasiniDinFisier()
        {
            if (File.Exists(fisierMasini))
            {
                string[] lines = File.ReadAllLines(fisierMasini);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length >= 7)
                    {
                        // Skip exited cars
                        if (line.Contains(";IESIT;")) continue;

                        int locIndex = int.Parse(parts[5]) - 1;
                        if (locIndex >= 0 && locIndex < locuri.Length)
                        {
                            var masina = new DetaliiMasina(parts[0], parts[1], parts[2], parts[3]);
                            DateTime dataIntrare = DateTime.Parse(parts[4]);
                            bool isVIP = parts[6] == "VIP";

                            masiniParcate[locIndex] = masina;
                            locuri[locIndex].OcupaLoc();
                            locuri[locIndex].DataOcupare = dataIntrare;
                            locuri[locIndex].VIP = isVIP;
                            bilete[locIndex] = new Bilete(masina, locuri[locIndex]);
                        }
                    }
                }
            }
        }

        public Bilete ParcheazaMasina(DetaliiMasina masina)
        {
            return ParcheazaMasina(masina, false);
        }

        public Bilete ParcheazaMasinaVIP(DetaliiMasina masina)
        {
            return ParcheazaMasina(masina, true);
        }

        private Bilete ParcheazaMasina(DetaliiMasina masina, bool isVIP)
        {
            for (int i = 0; i < locuri.Length; i++)
            {
                if (!locuri[i].Ocupat)
                {
                    locuri[i].OcupaLoc();
                    if (isVIP) locuri[i].VIP = true;

                    masiniParcate[i] = masina;
                    bilete[i] = new Bilete(masina, locuri[i]);

                    // Salvare in fisier
                    using (StreamWriter sw = new StreamWriter(fisierMasini, true))
                    {
                        sw.WriteLine($"{masina.NumarInmatriculare};{masina.Proprietar};{masina.Marca};{masina.Culoare};{DateTime.Now};{i + 1};{(isVIP ? "VIP" : "NORMAL")}");
                    }

                    Security.Log($"Masina {masina.NumarInmatriculare} parcata pe locul {i + 1} {(isVIP ? "VIP" : "")}");
                    return bilete[i];
                }
            }
            return null;
        }

        public string InregistreazaMasinaInteractiv(string nr, string prop, string marca, string culoare, bool vip)
        {
            DetaliiMasina masina = new DetaliiMasina(nr, prop, marca, culoare);
            var tichet = vip ? ParcheazaMasinaVIP(masina) : ParcheazaMasina(masina);

            if (tichet != null)
            {
                return $"Masina a fost parcata pe locul {tichet.NumarLoc}\n{tichet}";
            }
            return "Nu sunt locuri disponibile!";
        }

        public string AfiseazaPretCurentInteractiv(string nrInmatriculare)
        {
            var tichet = GetTichet(nrInmatriculare);
            if (tichet != null)
            {
                return $"Pret curent:\n" +
                       $"- RON: {tichet.CalculeazaCost(Valuta.RON):F2}\n" +
                       $"- EUR: {tichet.CalculeazaCost(Valuta.EUR):F2}\n" +
                       $"- USD: {tichet.CalculeazaCost(Valuta.USD):F2}";
            }
            return "Masina nu a fost gasita in parcare!";
        }

        public string ElibereazaSiPlatesteInteractiv(string nrInmatriculare, string modPlata, string valuta)
        {
            ModPlata mp = modPlata == "Cash" ? ModPlata.Cash : ModPlata.Card;
            Valuta v = valuta == "RON" ? Valuta.RON : (valuta == "EUR" ? Valuta.EUR : Valuta.USD);

            for (int i = 0; i < masiniParcate.Length; i++)
            {
                if (masiniParcate[i] != null && masiniParcate[i].NumarInmatriculare == nrInmatriculare)
                {
                    double cost = bilete[i].CalculeazaCost(v);
                    bilete[i].EfectueazaPlata(mp, v, cost);

                    // Actualizare fisier
                    string[] lines = File.ReadAllLines(fisierMasini);
                    for (int j = 0; j < lines.Length; j++)
                    {
                        if (lines[j].StartsWith(nrInmatriculare + ";") && !lines[j].Contains(";IESIT;"))
                        {
                            lines[j] += $";IESIT;{DateTime.Now};{cost:F2}";
                            break;
                        }
                    }
                    File.WriteAllLines(fisierMasini, lines);

                    // Eliberare loc
                    masiniParcate[i] = null;
                    locuri[i].ElibereazaLoc();
                    bilete[i] = null;

                    return $"Plata efectuata:\nSuma: {cost:F2} {valuta}\nMetoda: {modPlata}";
                }
            }
            return "Masina nu a fost gasita in parcare!";
        }

        public DetaliiMasina GetMasina(string nrInmatriculare)
        {
            foreach (var masina in masiniParcate)
            {
                if (masina != null && masina.NumarInmatriculare == nrInmatriculare)
                {
                    return masina;
                }
            }
            return null;
        }

        public Bilete GetTichet(string nrInmatriculare)
        {
            for (int i = 0; i < masiniParcate.Length; i++)
            {
                if (masiniParcate[i] != null && masiniParcate[i].NumarInmatriculare == nrInmatriculare)
                {
                    return bilete[i];
                }
            }
            return null;
        }

        public string AfiseazaMasiniParcate()
        {
            string result = "MASINI IN PARCARE:\n";
            bool existaMasini = false;

            for (int i = 0; i < masiniParcate.Length; i++)
            {
                if (masiniParcate[i] != null)
                {
                    existaMasini = true;
                    result += $"Loc {i + 1}: {masiniParcate[i]} {(locuri[i].VIP ? "(VIP)" : "")}\n";

                    if (locuri[i].Ocupat && locuri[i].DataOcupare != DateTime.MinValue)
                    {
                        TimeSpan durata = DateTime.Now - locuri[i].DataOcupare;
                        result += $"Durata: {durata.TotalMinutes:F1} minute\n";
                        result += $"Pret curent: {bilete[i].CalculeazaCost(Valuta.RON):F2} RON\n";
                    }
                    result += "----------------------------\n";
                }
            }

            if (!existaMasini)
            {
                result += "Nu am masini in parcare";
            }

            return result;
        }

        public List<DetaliiMasina> GetMasini()
        {
            return masiniParcate
                .Where(m => m != null)
                .ToList();
        }

       
        public string[] GetNumereMasini()
        {
            return GetMasini()
                .Select(m => m.NumarInmatriculare)
                .ToArray();
        }

       
        public Loc GetLoc(string numarInmatriculare)
        {
            for (int i = 0; i < masiniParcate.Length; i++)
                if (masiniParcate[i]?.NumarInmatriculare == numarInmatriculare)
                    return locuri[i];
            return null;
        }

       
        public bool UpdateMasina(DetaliiMasina masinaMod, bool vip)
        {
            for (int i = 0; i < masiniParcate.Length; i++)
            {
                var m = masiniParcate[i];
                if (m != null && m.NumarInmatriculare == masinaMod.NumarInmatriculare)
                {
                  
                    m.Proprietar = masinaMod.Proprietar;
                    m.Marca = masinaMod.Marca;
                    m.Culoare = masinaMod.Culoare;
                    m.DataActualizare = DateTime.Now;
                    locuri[i].VIP = vip;
                    SaveToFile();
                    return true;
                }
            }
            return false;
        }

       
        private void SaveToFile()
        {
            using (var sw = new StreamWriter(fisierMasini, false))
            {
                for (int i = 0; i < masiniParcate.Length; i++)
                {
                    var m = masiniParcate[i];
                    if (m == null) continue;
                    var loc = locuri[i];
                    var vip = loc.VIP ? "VIP" : "NORMAL";
                    sw.WriteLine(
                        $"{m.NumarInmatriculare};{m.Proprietar};{m.Marca};{m.Culoare};" +
                        $"{loc.DataOcupare};{i + 1};{vip}"
                    );
                }
            }
        }

    }
}
