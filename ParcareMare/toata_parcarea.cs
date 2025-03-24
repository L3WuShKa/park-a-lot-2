using System;
using LocParcare;
using Masina;
using Tichet;
using Securitate;
using System.IO;

namespace ParcareMare
{
    public class toata_parcarea
    {
        private Loc[] locuri; // tablou de locuri
        private DetaliiMasina[] masiniParcate; // tablou de masini parcate
        private Bilete[] bilete; // tablu de bilete, 
        private const string fisierMasini = "masini.txt";

        public toata_parcarea(int numarLocuri) //merge sa folosesc nume de motada asa chiar daca folosesc acelsi nume ca si clasa
        {
            locuri = new Loc[numarLocuri];
            masiniParcate = new DetaliiMasina[numarLocuri];
            bilete = new Bilete[numarLocuri];

            for (int i = 0; i < numarLocuri; i++)
            {
                locuri[i] = new Loc(i + 1); // ce se intampla : aloc un loc nou si ii dau numarul i+1
            }

            // citesc din fisier detalii despre masinile parcate etcccc
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
                    if (parts.Length >= 6 && !line.Contains(";IESIT;"))// daca linia are cel putin 6 elemente si nu contine "IESIT"
                    {
                        int locIndex = int.Parse(parts[5]) - 1; // indexul e cu 1 mai mic decat numarul locului 
                        if (locIndex >= 0 && locIndex < locuri.Length)
                        {
                            var masina = new DetaliiMasina(parts[0], parts[1], parts[2], parts[3]);
                            DateTime dataIntrare = DateTime.Parse(parts[4]);

                            masiniParcate[locIndex] = masina;
                            locuri[locIndex].OcupaLoc();
                            bilete[locIndex] = new Bilete(masina, locuri[locIndex], dataIntrare);
                        }
                    }
                }
            }
        }

        public Bilete ParcheazaMasina(DetaliiMasina masina)
        {
            for (int i = 0; i < locuri.Length; i++)
            {
                if (!locuri[i].Ocupat)
                {
                    locuri[i].OcupaLoc();
                    masiniParcate[i] = masina;
                    bilete[i] = new Bilete(masina, locuri[i], DateTime.Now);

                    // salvare in fisier
                    using (StreamWriter sw = new StreamWriter(fisierMasini, true))
                    {
                        sw.WriteLine($"{masina.NumarInmatriculare};{masina.Proprietar};{masina.Marca};{masina.Culoare};{DateTime.Now};{i + 1}");
                    }

                    Security.Log($"Masina {masina.NumarInmatriculare} parcata pe locul {i + 1}"); //imi scrie intr-un fisier log actiunea, dezvoltat in security.cs
                    return bilete[i];
                }
            }
            Console.WriteLine("Nu sunt locuri disponibile!");
            return null;
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
                    bilete[i] = new Bilete(masina, locuri[i], DateTime.Now);

                    // salvacza in fisier
                    using (StreamWriter sw = new StreamWriter(fisierMasini, true))
                    {
                        sw.WriteLine($"{masina.NumarInmatriculare};{masina.Proprietar};{masina.Marca};{masina.Culoare};{DateTime.Now};{i + 1};{(isVIP ? "VIP" : "NORMAL")}");
                    }

                    Security.Log($"Masina {masina.NumarInmatriculare} parcata pe locul {i + 1} {(isVIP ? "VIP" : "")}");
                    return bilete[i];
                }
            }
            Console.WriteLine("Nu sunt locuri disponibile!");
            return null;
        }

        public void ElibereazaLocSiPlateste(string nrInmatriculare, ModPlata modPlata, Valuta valuta)
        {
            for (int i = 0; i < masiniParcate.Length; i++)
            {
                if (masiniParcate[i] != null && masiniParcate[i].NumarInmatriculare == nrInmatriculare)
                {
                    double cost = bilete[i].CalculeazaCost(valuta);
                    bilete[i].EfectueazaPlata(modPlata, valuta, cost);

                    // actualizam fisierul si marcam ca am iesit
                    string[] lines = File.ReadAllLines(fisierMasini);
                    for (int j = 0; j < lines.Length; j++)
                    {
                        if (lines[j].StartsWith(nrInmatriculare + ";") && !lines[j].Contains(";IESIT;")) // daca linia incepe cu nr de inmatriculare si nu contine "IESIT"
                        {
                            lines[j] += $";IESIT;{DateTime.Now};{cost:F2}"; // adaug la linie "IESIT" si data curenta si costul ca asa am eu formatul si il respect
                            break;
                        }
                    }
                    File.WriteAllLines(fisierMasini, lines);

                    // eliberare loc
                    masiniParcate[i] = null;
                    locuri[i].ElibereazaLoc();
                    bilete[i] = null;

                    return;
                }
            }
            Console.WriteLine("Masina nu a fost gasita!");
        }

        public DetaliiMasina GetMasina(string nrInmatriculare)
        { // opt meniu
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

        public Bilete ParcheazaMasinaVIP(DetaliiMasina masina)
        {
            return ParcheazaMasina(masina, true); //practic apelez metoda de mai sus cu un parametru in plus fiindca am nevoie de un bool in plus
        }

    }
}