using System;
using ParcareMare;
using Masina;
using Securitate;
using Administrator;
using Tichet;
using System.IO;

namespace Parking
{
    class Program
    {
        static void Main()
        {
            toata_parcarea parcare = new toata_parcarea(10);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("MENIU PRINCIPAL");
                Console.WriteLine("1. Inregistreaza masina in parcare");
                Console.WriteLine("2. Afiseaza pret curent pentru masina");
                Console.WriteLine("3. Elibereaza loc si plateste");
                Console.WriteLine("4. Acces administrator");
                Console.WriteLine("5. Iesire");
                Console.Write("Alegeti optiunea: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        InregistreazaMasina(parcare);
                        break;
                    case "2":
                        AfiseazaPretCurent(parcare);
                        break;
                    case "3":
                        ElibereazaSiPlateste(parcare);
                        break;
                    case "4":
                        AccesAdmin();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("optiune invalida!");
                        break;
                }
                Console.WriteLine("\n apasati orice tastea pentru a continua");
                Console.ReadKey();
            }
        }

        static void InregistreazaMasina(toata_parcarea parcare)
        {
            Console.Clear();
            Console.WriteLine("INREGISTRARE MASINA");
            Console.Write("Numar inmatriculare: ");
            string nr = Console.ReadLine();
            Console.Write("Proprietar: ");
            string prop = Console.ReadLine();
            Console.Write("Marca: ");
            string marca = Console.ReadLine();
            Console.Write("Culoare: ");
            string culoare = Console.ReadLine();

            Console.Write("Doriti parcare VIP? (D/N): ");
            bool vip = Console.ReadLine().ToUpper() == "D"; //daca ce citesc e D atunci vip e true(ce citesc e al doilea ement din ecuttia asta minunata

            DetaliiMasina masina = new DetaliiMasina(nr, prop, marca, culoare);
            var tichet = vip ? parcare.ParcheazaMasinaVIP(masina) : parcare.ParcheazaMasina(masina);//var practic e un tip de date care se deduce din valoarea pe care o primeste
            //de ce am facut asa? apelul aclor 2 metode e acelasi doar ca una e vip si una nu
            if (tichet != null)
            {
                Console.WriteLine($"\nMasina a fost parcata pe locul {tichet.NumarLoc}");
                Console.WriteLine(tichet);
            }
        }

        static void AfiseazaPretCurent(toata_parcarea parcare)
        { //pt opt. din meniu user
            Console.Clear();
            Console.WriteLine("AFISARE PRET CURENT");
            Console.Write("Introduceti numarul de inmatriculare: ");
            string nr = Console.ReadLine();

            var masina = parcare.GetMasina(nr);
            if (masina != null)
            {
                var tichet = parcare.GetTichet(nr);
                Console.WriteLine($"\nPret curent: {tichet.CalculeazaCost(Valuta.RON):F2} RON");
                Console.WriteLine($"Echivalent: {tichet.CalculeazaCost(Valuta.EUR):F2} EUR");
                Console.WriteLine($"Echivalent: {tichet.CalculeazaCost(Valuta.USD):F2} USD");
            }
            else
            {
                Console.WriteLine("Masina nu a fost gasita in parcare!");
            }
        }

        static void ElibereazaSiPlateste(toata_parcarea parcare)
        {//pt opt. 3 din meniu user
            Console.Clear();
            Console.WriteLine("ELIBERARE LOC SI PLATA");
            Console.Write("Introduceti numarul de inmatriculare: ");
            string nr = Console.ReadLine();

            var masina = parcare.GetMasina(nr);
            //too lazy for a switch
           
            if (masina != null)
            {
                Console.WriteLine("\nAlegeti metoda de plata:");
                Console.WriteLine("1. Cash");
                Console.WriteLine("2. Card");
                Console.Write("Optiune: ");
                string optPlata = Console.ReadLine();

                Console.WriteLine("\nAlegeti valuta:");
                Console.WriteLine("1. RON");
                Console.WriteLine("2. EUR");
                Console.WriteLine("3. USD");
                Console.Write("Optiune: ");
                string optValuta = Console.ReadLine();

                ModPlata modPlata = optPlata == "1" ? ModPlata.Cash : ModPlata.Card;
                Valuta valuta = optValuta == "1" ? Valuta.RON : (optValuta == "2" ? Valuta.EUR : Valuta.USD);

                parcare.ElibereazaLocSiPlateste(nr, modPlata, valuta);
            }
            else
            {
                Console.WriteLine("Masina nu a fost gasita in parcare!");
            }
        }

        static void AccesAdmin()
        {
            Console.Clear();
            Console.WriteLine("ACCES ADMINISTRATOR");
            Console.Write("Nume utilizator: ");
            string user = Console.ReadLine();
            Console.Write("Parola: ");
            string pass = Console.ReadLine();

            if (Security.VerificaAcces(user, pass))
            {
                Admin.MeniuAdmin();
            }
            else
            {
                Console.WriteLine("Acces respins!");
            }
        }
    }
}