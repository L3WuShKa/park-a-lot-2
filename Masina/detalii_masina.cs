using System;

namespace Masina
{
    public class DetaliiMasina
    {
        public string NumarInmatriculare { get; set; }
        public string Proprietar { get; set; }
        public string Marca { get; set; }
        public string Culoare { get; set; }

        public DetaliiMasina(string numar, string proprietar, string marca, string culoare)
        {
            NumarInmatriculare = numar;
            Proprietar = proprietar;
            Marca = marca;
            Culoare = culoare;
        }

        public override string ToString()
        {
            return $"Masina {NumarInmatriculare} (Marca: {Marca}, Culoare: {Culoare}), Proprietar: {Proprietar}"; // de ce? ca sa pot afisa detaliile masinii in meniu
        }
    }
}