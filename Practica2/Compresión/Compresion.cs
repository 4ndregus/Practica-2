using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica2.Compresión
{
    public class Compresion
    {
        public List<string> entradas = new List<string>();
        public List<string> listCodificado = new List<string>();
        
        public Compresion(){}
        public List<string> codificar(string clave)
        {
            listCodificado.Clear();
            entradas.Clear();
            string entrada = "";
            int llave = 0;
            bool repetido = false;

            listCodificado.Add($"0");
            listCodificado.Add($"{clave[0]}");
            entradas.Add("");
            entradas.Add(clave[0] + "");

            for (int i = 1; i < clave.Length; i++)
            {
                entrada = entrada + clave[i];

                if (entradas.IndexOf(entrada) != -1)//No encontró entrada igual
                {
                    llave = entradas.IndexOf(entrada); //Buscar llave
                    repetido = true;

                    if (i + 1 == clave.Length)
                    {
                        listCodificado.Add($"{llave}");
                        listCodificado.Add("EOF");
                        listCodificado.Add("");
                    }

                }
                else //Encontró entrada igual
                {
                    if (repetido)
                    {
                        listCodificado.Add($"{llave}");
                        listCodificado.Add($"{entrada[entrada.Length - 1]}");
                    }
                    else
                    {
                        listCodificado.Add($"0");
                        listCodificado.Add($"{entrada}");
                    }
                    entradas.Add(entrada);
                    entrada = "";

                    repetido = false;
                }

            }
            
            return listCodificado;
        }

        public List<string> listEntradas()
        {
            return entradas;
        }

        public string decodificar(List<string> listCodificado, List<string> entradas)
        {
            string nextTexto;
            string decodificado = "";
            int direccion;
            for (int i = 0; i < listCodificado.Count; i += 2)
            {
                if (listCodificado[i].Length == 0)
                {
                    break;
                }
                direccion = Convert.ToInt32(listCodificado[i]);

                nextTexto = listCodificado[i + 1];
                if (nextTexto != "EOF")
                {
                    decodificado = decodificado + entradas[direccion] + nextTexto;
                }
                else
                {
                    decodificado = decodificado + entradas[direccion];
                }
                direccion = 0;
                nextTexto = "";
            }

            return decodificado;
        }
    }
}
