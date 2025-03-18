using System;

namespace Masina
{
    public class DetaliiMasina
    {
        public string NumarInmatriculare { get; set; }
        public string Proprietar { get; set; }

        public DetaliiMasina(string numar, string proprietar)
        {
            NumarInmatriculare = numar;
            Proprietar = proprietar;
        }

        public override string ToString()
        {
            return $"Masina {NumarInmatriculare}, Proprietar: {Proprietar}";
        }
    }
}