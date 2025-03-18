using System;
using System.IO;
using Securitate;
using ParcareMare;

namespace Administrator
{
    public class Admin
    {
        public static void AfiseazaLogSecuritate()
        {
            if (File.Exists(Security.FisierLog))
            {
                using (StreamReader sr = new StreamReader(Security.FisierLog))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            else
            {
                Console.WriteLine("Nu exista loguri de securitate.");
            }
        }

        public static void StergeLogSecuritate()
        {
            if (File.Exists(Security.FisierLog))
            {
                File.Delete(Security.FisierLog);
                Console.WriteLine("Logurile de securitate au fost sterse.");
            }
            else
            {
                Console.WriteLine("Nu exista loguri de securitate.");
            }
        }

        public static void GestioneazaLocuri(toata_parcarea parcare)
        {
            Console.WriteLine("Introduceti numarul locului pe care doresti să il gestionezi:");
            int numarLoc = int.Parse(Console.ReadLine());

            Console.WriteLine("Alege: (B)locare, (E)liberare");
            string optiune = Console.ReadLine().ToUpper();

            switch (optiune)
            {
                case "B":
                    Security.BlocheazaLoc(numarLoc);
                    break;
                case "E":
                    parcare.ElibereazaLoc(numarLoc);
                    break;
                default:
                    Console.WriteLine("Optiune invalida.");
                    break;
            }
        }
    }
}