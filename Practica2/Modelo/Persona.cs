using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica2.Modelo
{
    public class Persona
    {
        public string name { get; set; }
        public string dpi { get; set; }
        public string datebirth { get; set; }
        public string address { get; set; }
        public string[] companies { get; set; }

        public Persona(string name, string dpi, string datebirth, string address, string[] companies)
        {
            this.name = name;
            this.dpi = dpi;
            this.datebirth = datebirth;
            this.address = address;
            this.companies = companies;
        }

        public Comparison<Persona> CompararNombre = delegate (Persona persona1, Persona persona2)
        {
            return persona1.name.CompareTo(persona2.name);
        };

        public Comparison<Persona> CompararDpi = delegate (Persona persona1, Persona persona2)
        {
            return persona1.dpi.CompareTo(persona2.dpi);
        };

    }
}
