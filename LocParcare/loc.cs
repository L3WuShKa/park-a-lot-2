using System;
using Securitate;

namespace LocParcare
{
    public class Loc
    {
        public int NumarLoc { get; set; }
        public bool Ocupat { get; private set; }

        public Loc(int numar)
        {
            NumarLoc = numar;
            Ocupat = false;
        }

        public void OcupaLoc()
        {
            if (!Ocupat)
            {
                Ocupat = true;
                Security.Log($"Locul {NumarLoc} a fost ocupat.");
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
        }
    }
}