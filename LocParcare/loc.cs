using System;
using Securitate;

namespace LocParcare
{
    public class Loc
    {
        public int NumarLoc { get; set; }
        public bool Ocupat { get; private set; }
        public bool VIP { get; set; }
        public DateTime DataOcupare { get; set; }

        public Loc(int numar)
        {
            NumarLoc = numar;
            Ocupat = false;
            VIP = false;
            DataOcupare = DateTime.MinValue;
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
                Security.Log($"Încercare de ocupare a locului deja ocupat {NumarLoc}.");
            }
        }

        public void ElibereazaLoc()
        {
            Ocupat = false;
            VIP = false;
            DataOcupare = DateTime.MinValue;
            Security.Log($"Locul {NumarLoc} a fost eliberat.");
        }
    }
}
