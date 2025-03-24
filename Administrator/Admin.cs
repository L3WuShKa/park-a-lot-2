using System;
using System.IO;
using Securitate;
using ParcareMare;
using Tichet;

namespace Administrator
{
    public class Admin
    {
        public static void MeniuAdmin()
        {
            string optiune;
            do
            {
                Console.Clear();
                Console.WriteLine("MENIU ADMINISTRATOR");
                Console.WriteLine("1. Afisare log securitate");
                Console.WriteLine("2. Sterge log securitate");
                Console.WriteLine("3. Afisare masini in parcare");
                Console.WriteLine("4. Afisare istoric plati");
                Console.WriteLine("5. Inapoi");
                Console.Write("Optiune: ");

                optiune = Console.ReadLine();

                switch (optiune)
                {
                    case "1":
                        AfiseazaLogSecuritate();
                        break;
                    case "2":
                        StergeLogSecuritate();
                        break;
                    case "3":
                        AfiseazaMasiniInParcare();
                        break;
                    case "4":
                        AfiseazaIstoricPlati();
                        break;
                }
                if (optiune != "5")
                {
                    Console.WriteLine("\nApasati orice tasta pentru a continua...");
                    Console.ReadKey();
                }
            } while (optiune != "5");
        }

        static void AfiseazaLogSecuritate()
        {
            Console.WriteLine("\nLOGURI SECURITATE:");
            if (File.Exists(Security.FisierLog))
            {
                Console.WriteLine(File.ReadAllText(Security.FisierLog)); //citesc deoadata tot fisierul
            }
            else
            {
                Console.WriteLine("Nu exista loguri!");
            }
        }

        static void StergeLogSecuritate()
        {
            if (File.Exists(Security.FisierLog))
            {
                File.Delete(Security.FisierLog);
                Console.WriteLine("Logurile au fost sterse!");
            }
            else
            {
                Console.WriteLine("Nu exista loguri!");
            }
        }


        static void AfiseazaMasiniInParcare()
        {
            Console.WriteLine("\nMASINI IN PARCARE:");
            if (File.Exists("masini.txt"))
            {
                string[] lines = File.ReadAllLines("masini.txt");
                foreach (string line in lines)
                {
                    if (!line.Contains(";")) continue; // imi da skip la liniile goale sau care nu contin;
                    string[] parts = line.Split(';');

                    
                    if (parts.Length >= 7 && !line.Contains(";IESIT;"))
                    {
                        bool isVIP = parts[6] == "VIP"; // ce face e ca daca  parts[6] e "VIP" atunci isvip e true 
                        Console.WriteLine($"Nr: {parts[0]} - Loc: {parts[5]} - Tip: {(isVIP ? "VIP" : "Normal")}");
                        Console.WriteLine($"  Intrare: {parts[4]}");

                        // Calculate current price
                        DateTime intrare = DateTime.Parse(parts[4]);// parse imi transforma stringul in data de calendar.
                        TimeSpan durata = DateTime.Now - intrare;
                        double pretRON = durata.TotalSeconds * (isVIP ? 14.0 : 7.0); 
                        Console.WriteLine($"  Pret curent: {pretRON:F2} RON ({pretRON * 0.2:F2} EUR)");// f2 imi arata 2 zecimale supa punct
                        Console.WriteLine("----------------------------------");
                    }
                }
            }
            else
            {
                Console.WriteLine("Nu exista masini in parcare!");
            }
        }


        static void AfiseazaIstoricPlati()
        {
            Console.WriteLine("\nISTORIC PLATI:");
            if (File.Exists(Bilete.FisierPlati))
            {
                string[] lines = File.ReadAllLines(Bilete.FisierPlati);
                foreach (string line in lines)
                {
                    if (!line.Contains(";")) continue;
                    string[] parts = line.Split(';');
                    Console.WriteLine($"{parts[0]} - {parts[1]} - {parts[2]} {parts[4]} {parts[3]}");
                }
            }
            else
            {
                Console.WriteLine("Nu exista plati!");
            }
        }
    }
}