using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica2.Estructura
{
    public class AVL<T>
    {
        private Nodo<T> raiz;
        public List<T> lista;
        public List<T> listaBusqueda;
        public List<T> datoBusqueda;
        public AVL()
        {
            raiz = null;
            lista = new List<T>();
            listaBusqueda = new List<T>();
        }

        //Insertar
        public void insertar(T valor, Delegate delegado1)
        {
            raiz = insertarAVL(raiz, valor, delegado1);
            inOrder(raiz);
        }

        //Eliminar
        public void eliminar(T valor, Delegate delegado1)
        {
            raiz = eliminarAVL(raiz, delegado1, valor);
            inOrder(raiz);
        }

        //Altura del árbol
        private int Altura(Nodo<T> actual)
        {
            if (actual == null)
            {
                return 0;
            }
            return actual.FE;
        }

        //Obtener Factor de Equilibrio
        private int obtenerFE(Nodo<T> actual)
        {
            if (actual == null)
            {
                return 0;
            }
            return Altura(actual.nodoIzq) - Altura(actual.nodoDer);
        }

        //Rotaciones
        public Nodo<T> rotacionSimpleIzquierda(Nodo<T> A)
        {
            Nodo<T> aux = A.nodoDer;
            Nodo<T> aux2 = aux.nodoIzq;
            aux.nodoIzq = A;
            A.nodoDer = aux2;

            A.FE = Math.Max(Altura(A.nodoIzq), Altura(A.nodoDer)) + 1;
            aux.FE = Math.Max(Altura(aux.nodoIzq), Altura(aux.nodoDer)) + 1;
            return aux;
        }

        public Nodo<T> rotacionSimpleDerecha(Nodo<T> A)
        {
            Nodo<T> aux = A.nodoIzq;
            Nodo<T> aux2 = aux.nodoDer;
            aux.nodoDer = A;
            A.nodoIzq = aux2;

            A.FE = Math.Max(Altura(A.nodoIzq), Altura(A.nodoDer)) + 1;
            aux.FE = Math.Max(Altura(aux.nodoIzq), Altura(aux.nodoDer)) + 1;
            return aux;
        }

        public Nodo<T> rotacionDobleIzquierda(Nodo<T> A)
        {
            Nodo<T> aux;
            A.nodoDer = rotacionSimpleDerecha(A.nodoDer);
            aux = rotacionSimpleIzquierda(A);
            return aux;
        }

        public Nodo<T> rotacionDobleDerecha(Nodo<T> A)
        {
            Nodo<T> aux;
            A.nodoIzq = rotacionSimpleIzquierda(A.nodoIzq);
            aux = rotacionSimpleDerecha(A);
            return aux;
        }

        //Modificar
        public void modificar(Nodo<T> buscado, Delegate delegado1)
        {
            Nodo<T> aux = busqueda(raiz, delegado1, buscado);
            if (aux == null || buscado.valor == null)
            {
                listaBusqueda.Clear();
            }
            else
            {
                aux.valor = buscado.valor;
            }
            inOrder(raiz);
        }

        public Nodo<T> busqueda(Nodo<T> aux, Delegate delegado1, Nodo<T> buscado)
        {
            Nodo<T> resultado = null;
            if (aux == null || buscado.valor == null)
            {
                resultado = null;
            }
            else
            {
                if (Convert.ToInt32(delegado1.DynamicInvoke(aux.valor, buscado.valor)) == 0)
                {
                    resultado = aux;
                }
                else if (Convert.ToInt32(delegado1.DynamicInvoke(buscado.valor, aux.valor)) < 0)
                {
                    resultado = busqueda(aux.nodoIzq, delegado1, buscado);
                }
                else if (Convert.ToInt32(delegado1.DynamicInvoke(buscado.valor, aux.valor)) > 0)
                {
                    resultado = busqueda(aux.nodoDer, delegado1, buscado);
                }

            }
            return resultado;
        }

        //Búsqueda
        public void buscarr(Nodo<T> buscado, Delegate delegado1)
        {
            Nodo<T> aux = busqueda(raiz, delegado1, buscado);
            if (aux == null || buscado.valor == null)
            {
                listaBusqueda.Clear();
            }
            else
            {
                listaBusqueda.Add(busqueda(raiz, delegado1, buscado).valor);
            }
        }

        public Nodo<T> insertarAVL(Nodo<T> actual, T valor, Delegate delegado1)
        {
            if (actual == null)
            {
                return new Nodo<T>(valor);
            }
            if (Convert.ToInt32(delegado1.DynamicInvoke(valor, actual.valor)) < 0)
            {
                actual.nodoIzq = insertarAVL(actual.nodoIzq, valor, delegado1);
            }
            else if (Convert.ToInt32(delegado1.DynamicInvoke(valor, actual.valor)) > 0)
            {
                actual.nodoDer = insertarAVL(actual.nodoDer, valor, delegado1);
            }
            else
            {
                return actual;
            }

            actual.FE = 1 + Math.Max(Altura(actual.nodoIzq), Altura(actual.nodoDer));

            int FE = obtenerFE(actual);

            if (FE > 1 && Convert.ToInt32(delegado1.DynamicInvoke(valor, actual.nodoIzq.valor)) < 0)
            {
                return rotacionSimpleDerecha(actual);
            }
            if (FE < -1 && Convert.ToInt32(delegado1.DynamicInvoke(valor, actual.nodoDer.valor)) > 0)
            {
                return rotacionSimpleIzquierda(actual);
            }
            if (FE > 1 && Convert.ToInt32(delegado1.DynamicInvoke(valor, actual.nodoIzq.valor)) > 0)
            {

                return rotacionDobleDerecha(actual);
            }
            if (FE < -1 && Convert.ToInt32(delegado1.DynamicInvoke(valor, actual.nodoDer.valor)) < 0)
            {
                return rotacionDobleIzquierda(actual);
            }

            return actual;
        }

        //Recorridos
        public void inOrder(Nodo<T> r)
        {
            lista.Clear();
            Recorrido(r);
        }
        public void Recorrido(Nodo<T> r)
        {
            if (r != null)
            {
                Recorrido(r.nodoIzq);
                lista.Add(r.valor);
                Recorrido(r.nodoDer);
            }
        }
        public void buscar(T valor, Delegate delegado1)
        {
            listaBusqueda.Clear();
            for (int i = 0; i < lista.Count; i++)
            {
                if (Convert.ToInt32(delegado1.DynamicInvoke(valor, lista[i])) == 0)
                {
                    listaBusqueda.Add(lista[i]);
                }
            }
        }

        private Nodo<T> NodoConValorMin(Nodo<T> nodo)
        {
            Nodo<T> actual = nodo;
            while (actual.nodoIzq != null)
            {
                actual = actual.nodoIzq;
            }

            return actual;
        }

        private Nodo<T> eliminarAVL(Nodo<T> actual, Delegate delegado1, T valor)
        {
            if (actual == null)
            {
                return actual;
            }
            if (Convert.ToInt32(delegado1.DynamicInvoke(valor, actual.valor)) < 0)
            {
                actual.nodoIzq = eliminarAVL(actual.nodoIzq, delegado1, valor);
            }
            else if (Convert.ToInt32(delegado1.DynamicInvoke(valor, actual.valor)) > 0)
            {
                actual.nodoDer = eliminarAVL(actual.nodoDer, delegado1, valor);
            }
            else
            {
                //El nodo es igual al elemento y se elimina
                //Nodo con un único hijo o es hoja
                if ((actual.nodoIzq == null) || (actual.nodoDer == null))
                {
                    Nodo<T> temp = null;
                    if (temp == actual.nodoIzq)
                    {
                        temp = actual.nodoDer;
                    }
                    else
                    {
                        temp = actual.nodoIzq;
                    }

                    //No tiene hijos
                    if (temp == null)
                    {
                        temp = actual;
                        actual = null;
                    }
                    else
                    {
                        actual = temp; //Elimina el valor actual reemplazándolo por su hijo
                    }
                }
                else
                {
                    //Nodo con dos hijos, se busca el predecesor
                    Nodo<T> temp = NodoConValorMin(actual.nodoDer);

                    actual.valor = temp.valor;

                    actual.nodoDer = eliminarAVL(actual.nodoDer, delegado1, temp.valor);
                }
            }
            //Si solo tiene un nodo
            if (actual == null)
            {
                return actual;
            }

            //Actualiza altura
            actual.FE = Math.Max(Altura(actual.nodoIzq), Altura(actual.nodoDer)) + 1;

            int FE = obtenerFE(actual);

            if (FE > 1 && obtenerFE(actual.nodoIzq) >= 0)
            {
                return rotacionSimpleDerecha(actual);
            }

            if (FE > 1 && obtenerFE(actual.nodoIzq) < 0)
            {
                return rotacionDobleDerecha(actual);
            }

            if (FE < -1 && obtenerFE(actual.nodoDer) <= 0)
            {
                return rotacionSimpleIzquierda(actual);
            }

            if (FE < -1 && obtenerFE(actual.nodoDer) > 0)
            {
                return rotacionDobleIzquierda(actual);
            }

            return actual;
        }
    }
}
