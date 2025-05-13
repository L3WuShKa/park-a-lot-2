using System;
using System.IO;
using System.Linq;
using Common;

namespace Administrator
{
    public class Admin : IAdminAccess
    {
        private const string FisierMasini = "masini.txt";
        private const string FisierPlati = "plati.txt";
        private const string AdminPassword = "12345";

        public bool Authenticate(string password)
        {
            bool success = password == AdminPassword;
            Security.Log(success ? "Acces administrativ permis" : "Incercare esuata de acces administrativ");
            return success;
        }

        public void ShowAdminMenu()
        {
            AfiseazaLogSecuritate();
        }

        public void AfiseazaLogSecuritate()
        {
            Console.WriteLine("\n=== LOGURI SECURITATE ===");
            if (File.Exists(Security.FisierLog))
            {
                foreach (string line in File.ReadLines(Security.FisierLog))
                {
                    Console.WriteLine(line);
                }
            }
            else
            {
                Console.WriteLine("Nu exista loguri!");
            }
        }

        public void StergeLogSecuritate()
        {
            if (File.Exists(Security.FisierLog))
            {
                File.Delete(Security.FisierLog);
                Console.WriteLine("Logurile au fost sterse cu succes!");
            }
            else
            {
                Console.WriteLine("Nu exista fisier log!");
            }
        }

        public void AfiseazaMasiniInParcare()
        {
            Console.WriteLine("\n=== MASINI IN PARCARE ===");
            if (File.Exists(FisierMasini))
            {
                var masini = File.ReadAllLines(FisierMasini)
                    .Where(line => !line.Contains(";IESIT;"))
                    .ToList();

                if (masini.Any())
                {
                    Console.WriteLine("Nr. | Proprietar          | Marca       | Culoare   | Loc | Tip      | Intrare");
                    Console.WriteLine("----------------------------------------------------------------------------");
                    foreach (string line in masini)
                    {
                        string[] parts = line.Split(';');
                        if (parts.Length >= 7)
                        {
                            Console.WriteLine($"{parts[0],-4} | {parts[1],-19} | {parts[2],-11} | {parts[3],-9} | {parts[5],3} | {parts[6],-8} | {parts[4]}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Nu exista masini in parcare!");
                }
            }
            else
            {
                Console.WriteLine("Nu exista date despre masini!");
            }
        }

        public void AfiseazaIstoricPlati()
        {
            Console.WriteLine("\n=== ISTORIC PLATI ===");
            if (File.Exists(FisierPlati))
            {
                var plati = File.ReadAllLines(FisierPlati);
                if (plati.Any())
                {
                    Console.WriteLine("Data                | Nr. Masina  | Metoda   | Valuta | Suma    | Durata");
                    Console.WriteLine("---------------------------------------------------------------");
                    foreach (string line in plati)
                    {
                        string[] parts = line.Split(';');
                        if (parts.Length >= 6)
                        {
                            Console.WriteLine($"{parts[0],-19} | {parts[1],-11} | {parts[2],-8} | {parts[3],-6} | {parts[4],7} | {parts[5]}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Nu exista plati inregistrate!");
                }
            }
            else
            {
                Console.WriteLine("Nu exista istoric plati!");
            }
        }

        public void CautaMasina(string nrInmatriculare)
        {
            Console.WriteLine("\n=== REZULTATE CAUTARE ===");
            bool found = false;

            if (File.Exists(FisierMasini))
            {
                foreach (string line in File.ReadAllLines(FisierMasini))
                {
                    if (line.StartsWith(nrInmatriculare + ";"))
                    {
                        string[] parts = line.Split(';');
                        Console.WriteLine($"Numar: {parts[0]}");
                        Console.WriteLine($"Proprietar: {parts[1]}");
                        Console.WriteLine($"Marca: {parts[2]}");
                        Console.WriteLine($"Culoare: {parts[3]}");
                        Console.WriteLine($"Data intrare: {parts[4]}");
                        Console.WriteLine($"Loc parcare: {parts[5]}");
                        Console.WriteLine($"Tip: {(parts[6] == "VIP" ? "VIP" : "Standard")}");
                        if (parts.Length > 7) Console.WriteLine($"Data iesire: {parts[8]}");
                        found = true;
                        break;
                    }
                }
            }
            if (!found) Console.WriteLine($"Masina cu numarul {nrInmatriculare} nu a fost gasita!");
        }
    }
}
