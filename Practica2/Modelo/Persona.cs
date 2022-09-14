using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Practica2.Compresión;

namespace Practica2.Modelo
{
    public class Persona
    {
        public string name { get; set; }
        public string dpi { get; set; }
        public string datebirth { get; set; }
        public string address { get; set; }
        public string[] companies { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public List<List<string>[]> codificados { get; private set; } = new List<List<string>[]>();

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public List<List<string>[]> entradas {get; private set; } = new List<List<string>[]>();


        Compresion comprimido = new Compresion();
        

        public Persona(string name, string dpi, string datebirth, string address)
        {
            this.name = name;
            this.dpi = dpi;
            this.datebirth = datebirth;
            this.address = address;
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
