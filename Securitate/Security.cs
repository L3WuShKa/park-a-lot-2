using System;
using System.IO;

namespace Securitate
{
    public static class Security
    {
        public const string FisierLog = "log_securitate.txt";

        public static void Log(string mesaj)
        {
            using (StreamWriter sw = new StreamWriter(FisierLog, true))
            {
                sw.WriteLine($"{DateTime.Now}: {mesaj}");
            }
        }

        public static bool VerificaAcces(string numeUtilizator, string parola)
        {
            if (numeUtilizator == "admin" && parola == "parola123")
            {
                Log($"Acces permis pentru {numeUtilizator}");
                return true;
            }
            else
            {
                Log($"Acces respins pentru {numeUtilizator}");
                return false;
            }
        }

        public static void BlocheazaLoc(int numarLoc)
        {
            Log($"locul {numarLoc} a fost blocat din motive de securitate.");
            Console.WriteLine($"locul {numarLoc} a fost blocat.");
        }
    }
}