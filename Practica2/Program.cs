﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Practica2.Estructura;
using Practica2.Modelo;
using Practica2.Compresión;

namespace Practica2
{
    public class Program
    {
        public static AVL<Persona> AVLDpi;
        public static Compresion compresion = new Compresion();
        public static List<List<string>> listaCodificados = new List<List<string>>();

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
                    for (int i = 0; i < nuevaPersona.companies.Length; i++)
                    {
                        nuevaPersona.companies[i] = i + ": " + nuevaPersona.companies[i];
                    }
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
                    for (int i = 0; i < nuevaPersona.companies.Length; i++)
                    {
                        nuevaPersona.companies[i] = i + ": " + nuevaPersona.companies[i];
                    }
                    AVLDpi.modificar(nuevoNodo, nuevaPersona.CompararDpi);

                }
            }

            buscar();
            Console.ReadKey();
        }

        public static void menu()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n-----  Menú de Operaciones  -----");
            Console.WriteLine("1. Codificar");
            Console.WriteLine("2. Decodificar");
            Console.WriteLine("3. Seguir buscando\n");            
        }


        public static void buscar()
        {
            string dpi;
            Console.Clear();
            Console.Write("Ingrese DPI a buscar: ");
            dpi = Console.ReadLine();

            Persona busqueda = new Persona("", dpi, "", "");
            AVLDpi.buscar(busqueda, busqueda.CompararDpi);

            if (AVLDpi.listaBusqueda.Count() == 0)
            {
                Console.WriteLine("Persona no encontrada");
            }
            else
            {
                //Console.WriteLine("Búsqueda guardada");
                //Exportar archivo jsonl
                //string codificado = AVLDpi.listaBusqueda[0].companies[0];
                //string jsonl = JsonSerializer.Serialize(AVLDpi.listaBusqueda[0]);
                //File.WriteAllText($"{busqueda.dpi}.jsonl", codificado);               
            Menu:
                try
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"PERSONA ENCONTRADA:\n");

                    //Mostar json de la persona
                    var options = new JsonSerializerOptions {
                        IgnoreReadOnlyProperties = true,
                        WriteIndented = true                      
                    };

                    string jsonl = JsonSerializer.Serialize(AVLDpi.listaBusqueda[0], options);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(jsonl);

                    PedirEmpresa:
                    int numEmpresa;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\nSelecciona la empresa: ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    numEmpresa = int.Parse(Console.ReadLine());

                    if(numEmpresa > AVLDpi.listaBusqueda[0].companies.Length - 1)
                    {
                        Console.WriteLine("Selecciona una empresa dentro del rango");
                        goto PedirEmpresa;
                    }

                    string empresa = AVLDpi.listaBusqueda[0].companies[numEmpresa].Substring(AVLDpi.listaBusqueda[0].companies[numEmpresa].IndexOf(":") + 2);

                    int opcion;
                    menu();                    
                    opcion = int.Parse(Console.ReadLine());

                    if (opcion == 1) //Codificar
                    {
                        Console.Clear();
                        Console.WriteLine($"DPI ingresado: {busqueda.dpi}");
                        int apariciones = 0;
                        for (int i = 0; i < AVLDpi.listaBusqueda[0].companies.Length; i++)
                        {
                            if (AVLDpi.listaBusqueda[0].companies[i].Substring(AVLDpi.listaBusqueda[0].companies[i].IndexOf(":") + 2) == empresa)
                            {
                                apariciones++;
                                string codificado = "";
                                List<string> listCodificado = new List<string>();
                                List<string> entradas = new List<string>();
                                List<string> aux = new List<string>();
                                
                                listCodificado = compresion.codificar(AVLDpi.listaBusqueda[0].companies[i].Substring(AVLDpi.listaBusqueda[0].companies[i].IndexOf(":") + 2) + AVLDpi.listaBusqueda[0].dpi);
                                codificado = listCodificado.Aggregate((x, y) => x + y);
                                aux.Add(codificado);
                                entradas = compresion.listEntradas();
                                AVLDpi.listaBusqueda[0].companies[i] = AVLDpi.listaBusqueda[0].companies[i] + ": " + codificado;
                                
                                if(apariciones ==  1)
                                {
                                    List<string>[] tablaEntradas = { aux, entradas };
                                    AVLDpi.listaBusqueda[0].entradas.Add(tablaEntradas);
                                    List<string>[] tablaCodificacion = { aux, listCodificado };
                                    AVLDpi.listaBusqueda[0].codificados.Add(tablaCodificacion);
                                }
                               
                            }                           
                        }

                        Nodo<Persona> nuevoNodo = new Nodo<Persona>(AVLDpi.listaBusqueda[0]);
                        AVLDpi.modificar(nuevoNodo, AVLDpi.listaBusqueda[0].CompararDpi);
                        goto Menu;
                    }
                    else if (opcion == 2) //Decodificar
                    {
                        string DPIcodificado, decodificado = "";                     
                        Console.Write("Ingresa DPI codificado: ");
                        DPIcodificado = Console.ReadLine();
                        for (int i = 0; i < AVLDpi.listaBusqueda[0].codificados.Count; i++)
                        {
                            if (AVLDpi.listaBusqueda[0].codificados[i][0][0] == DPIcodificado && AVLDpi.listaBusqueda[0].entradas[i][0][0] == DPIcodificado)
                            {
                                decodificado = compresion.decodificar(AVLDpi.listaBusqueda[0].codificados[i][1], AVLDpi.listaBusqueda[0].entradas[i][1]);
                            }
                        }

                        Console.WriteLine(decodificado);

                    }
                    else if (opcion == 3) //Buscar
                    {
                        buscar();
                    }
                    else
                    {
                        Console.WriteLine("Ingrese una opción válida");
                        goto Menu;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Ingrese una opción válida");
                    goto Menu;
                }

            }
        }
    }
}
