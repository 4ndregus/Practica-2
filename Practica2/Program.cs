using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Practica2.Estructura;
using Practica2.Modelo;

namespace Practica2
{
    public class Program
    {
        public static AVL<Persona> AVLDpi;
        static void Main(string[] args)
        {
            string ubicacionArchivo = "C:\\Users\\agust\\OneDrive - Universidad Rafael Landivar\\URL\\6) Segundo Ciclo 2022\\Estructura de datos II\\Practica-2\\Practica2\\input.csv";
            System.IO.StreamReader archivo = new System.IO.StreamReader(ubicacionArchivo);
            string linea;

            AVLDpi = new AVL<Persona>();
            while ((linea = archivo.ReadLine()) != null)
            {
                string[] fila = linea.Split(';'); //Separador

                if (fila[0] == "INSERT")
                {
                    string json = fila[1];
                    Persona nuevaPersona = JsonSerializer.Deserialize<Persona>(json);
                    AVLDpi.insertar(nuevaPersona, nuevaPersona.CompararDpi);
                }
                if (fila[0] == "DELETE")
                {
                    string json = fila[1];
                    Persona nuevaPersona = JsonSerializer.Deserialize<Persona>(json);
                    AVLDpi.eliminar(nuevaPersona, nuevaPersona.CompararDpi);
                }
                if (fila[0] == "PATCH")
                {
                    string json = fila[1];
                    Persona nuevaPersona = JsonSerializer.Deserialize<Persona>(json);
                    Nodo<Persona> nuevoNodo = new Nodo<Persona>(nuevaPersona);
                    AVLDpi.modificar(nuevoNodo, nuevaPersona.CompararDpi);

                }
            }

            for(int i = 0; i < AVLDpi.lista.Count; i++)
            {
                Console.WriteLine($"name: {AVLDpi.lista[i].name} dpi: {AVLDpi.lista[i].dpi} datebirth: {AVLDpi.lista[i].datebirth} address: {AVLDpi.lista[i].address} companies: {AVLDpi.lista[i].companies[0]}");
            }
            Console.WriteLine(AVLDpi.lista.Count);

            Console.ReadKey();
        }

        public static void buscar(string dpi)
        {
            Console.Clear();
            Console.Write("Ingrese DPI a buscar: ");
            dpi = Console.ReadLine();

            //Persona busqueda = new Persona("", dpi, "", "");
        }
    }
}
