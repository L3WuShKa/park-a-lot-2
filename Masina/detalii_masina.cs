using System;

namespace Masina
{
    [Serializable]
    public class DetaliiMasina
    {
        public string NumarInmatriculare { get; set; }
        public string Proprietar { get; set; }
        public string Marca { get; set; }
        public string Culoare { get; set; }
        public DateTime DataIntrare { get; set; }
        public DateTime DataActualizare { get; set; }


        public DetaliiMasina(string numar, string proprietar, string marca, string culoare)
        {
            NumarInmatriculare = numar;
            Proprietar = proprietar;
            Marca = marca;
            Culoare = culoare;
            DataIntrare = DateTime.Now;
            DataActualizare = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Masina {NumarInmatriculare}, Proprietar: {Proprietar}, Marca: {Marca}, Culoare: {Culoare}, Intrare: {DataIntrare}, Actualizare: {DataActualizare}";
        }
    }
}
