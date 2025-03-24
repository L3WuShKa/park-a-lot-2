using System;
using Securitate;

namespace LocParcare
{
    public class Loc
    {
        public int NumarLoc { get; set; }
        public bool Ocupat { get; private set; }
        public bool VIP { get; set; }
        public DateTime DataOcupare { get; private set; }

        public Loc(int numar)
        {
            NumarLoc = numar;
            Ocupat = false;
            VIP = false;
        }

        public void OcupaLoc()
        {
            if (!Ocupat)
            {
                Ocupat = true;
                DataOcupare = DateTime.Now;
                Security.Log($"Locul {NumarLoc} a fost ocupat. VIP: {VIP}");
            }
            else
            {
                Console.WriteLine("Locul este deja ocupat.");
                Security.Log($"Încercare de ocupare a locului deja ocupat {NumarLoc}."); //scriu in log
            }
        }

        public void ElibereazaLoc()
        {
            Ocupat = false;
            VIP = false; //resetez vip-ul cand eliberez parcarea, de exemplu mai vin pe viitor inca odata si daca nu resetez, o sa ramana vip si nu e ok T~T
            Security.Log($"Locul {NumarLoc} a fost eliberat.");
        }
    }
}