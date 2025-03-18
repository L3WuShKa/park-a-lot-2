using System;
using ParcareMare;
using Masina;
using Securitate;
using Administrator;

namespace Parking
{
    class Program
    {
        static void Main()
        {
            toata_parcarea parcare = new toata_parcarea(5);
            DetaliiMasina masina1 = new DetaliiMasina("B-123-XYZ", "Andrei Popescu");
            DetaliiMasina masina2 = new DetaliiMasina("SV-42-SME", "Ruben Iacob Levi");
            var tichet = parcare.ParcheazaMasina(masina1);
            var tichet2 = parcare.ParcheazaMasina(masina2);

            if (tichet != null && tichet2 != null)
            {
                Console.WriteLine(tichet);
                Console.WriteLine(tichet2);
            }

            // Afisare toate masinile parcate
            Console.WriteLine("Masinile parcate:");
            parcare.AfisareMasini();

            // Caut masina
            var masinaGasita = parcare.GetMasina("B-123-XYZ");
            if (masinaGasita != null)
            {
                Console.WriteLine($"Masina găsită: {masinaGasita}");
            }

            // Eliberez loc
            parcare.ElibereazaLoc(1);

            // Meniu administrator
            Console.WriteLine("Introdu numele de utilizator si parola pentru acces administrator : (hint user : admin , pass: parola123 )");
            string numeUtilizator = Console.ReadLine();
            string parola = Console.ReadLine();

            if (Security.VerificaAcces(numeUtilizator, parola))
            {
                string optiune;
                do
                {
                    Console.WriteLine("A. afisare log securitate");
                    Console.WriteLine("S. Sterge log");
                    Console.WriteLine("G. Gestionare locuri");
                    Console.WriteLine("X. exit");

                    Console.WriteLine("Optiunea : ");
                    optiune = Console.ReadLine().ToUpper();

                    switch (optiune)
                    {
                        case "A":
                            Admin.AfiseazaLogSecuritate();
                            break;
                        case "S":
                            Admin.StergeLogSecuritate();
                            break;
                        case "G":
                            Admin.GestioneazaLocuri(parcare);
                            break;
                        case "X":
                            return;
                        default:
                            Console.WriteLine("Optiune inexistenta");
                            break;
                    }
                } while (optiune.ToUpper() != "X");
            }
            else
            {
                Console.WriteLine("Acces respins.");
            }
        }
    }
}