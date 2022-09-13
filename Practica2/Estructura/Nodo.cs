using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica2.Estructura
{
    public class Nodo<T>
    {
        public T valor;
        public int FE;
        public Nodo<T> nodoIzq, nodoDer;
        public Nodo(T valor)
        {
            FE = 1;
            nodoIzq = null;
            nodoDer = null;
            this.valor = valor;
        }
    }
}
