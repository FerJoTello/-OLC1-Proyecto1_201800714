using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201800714
{
    class TipoEstructura
    {
        public enum Tipo
        {
            OR,
            AND,
            KLEEN,
            TERMINAL
        }
    }
    /* Estructura:      Clase abstracta que permite analizar una ER al recuperar la info en el Analisis Sintactico.
     *                  Esto con el fin de controlar a cada Operador (or, and y kleen) e indicar su estructura sin ser especifico en qué es lo que se espera.
     *                  Por lo que cada Operador puede obtener otro Operador (or, and y kleen) o un terminal y así cumplir con la estructura que solicita cada uno.
     * Parametros:  ->  Tipo: indica qué tipo de Operador se está trabajando (or, and, kleen o terminal).
     *              ->  ContadorNodo: utilizado para llevar la correlación de cada nodo en la generación de los AFN Y AFD
     * Metodos:     ->  Ejecutar(int n): utilizado para generar acciones.
     *              ->  Numerar(int n): utilizado para enumerar cada nodo.
     */
    public abstract class Estructura
    {
        TipoEstructura Tipo;
        int ContadorNodo;
        Nodo First, Last;
        public void SetFirst(Nodo first)
        {
            First = first;
        }
        public void SetLast(Nodo last)
        {
            Last = last;
        }
        public Nodo GetFirst()
        {
            return First;
        }
        public Nodo GetLast()
        {
            return Last;
        }
        public abstract object Ejecutar(int n);
        public abstract object Numerar(ref int n);
    }
    public class Or : Estructura
    {
        Estructura Estructura1, Estructura2;
        Nodo n1, n2, n3, n4, n5, n6;
        public Or(Estructura estructura1, Estructura estructura2)
        {
            Estructura1 = estructura1;
            Estructura2 = estructura2;
        }
        public override object Ejecutar(int n)
        {
            return null;
        }

        public override object Numerar(ref int n)
        {
            Terminal epsilon = new Terminal(Terminal.Tipo.EPSILON, "");
            if (Estructura1 is Terminal term1)
            {
                n1 = new Nodo(n++);
                n2 = new Nodo(n++);
                Transicion tra1 = new Transicion(epsilon, n2);
                n1.Transiciones.AddLast(tra1);
                n3 = new Nodo(n++);
                Transicion tra2 = new Transicion(term1, n3);
                n2.Transiciones.AddLast(tra2);
                if (Estructura2 is Terminal term2)
                {
                    n4 = new Nodo(n++);
                    Transicion tra3 = new Transicion(epsilon, n4);
                    n1.Transiciones.AddLast(tra3);
                    n5 = new Nodo(n++);
                    Transicion tra4 = new Transicion(term2, n5);
                    n4.Transiciones.AddLast(tra4);
                    n6 = new Nodo(n++);
                    Transicion tra5 = new Transicion(epsilon, n6);
                    n3.Transiciones.AddLast(tra5);
                    n5.Transiciones.AddLast(tra5);
                }
                else
                {
                    Estructura2.Numerar(ref n);
                    n4 = Estructura2.GetFirst();
                    Transicion tra3 = new Transicion(epsilon, n4);
                    n1.Transiciones.AddLast(tra3);
                    n5 = Estructura2.GetLast();
                    n6 = new Nodo(n++);
                    Transicion tra5 = new Transicion(epsilon, n6);
                    n3.Transiciones.AddLast(tra5);
                    n5.Transiciones.AddLast(tra5);
                }
            }
            else
            {
                n1 = new Nodo(n++);
                Estructura1.Numerar(ref n);
                n2 = Estructura1.GetFirst();
                Transicion tra1 = new Transicion(epsilon, n2);
                n1.Transiciones.AddLast(tra1);
                n3 = Estructura1.GetLast();
                if (Estructura2 is Terminal term2)
                {
                    n4 = new Nodo(n++);
                    Transicion tra3 = new Transicion(epsilon, n4);
                    n1.Transiciones.AddLast(tra3);
                    n5 = new Nodo(n++);
                    Transicion tra4 = new Transicion(term2, n5);
                    n4.Transiciones.AddLast(tra4);
                    n6 = new Nodo(n++);
                    Transicion tra5 = new Transicion(epsilon, n6);
                    n3.Transiciones.AddLast(tra5);
                    n5.Transiciones.AddLast(tra5);
                }
                else
                {
                    Estructura2.Numerar(ref n);
                    n4 = Estructura2.GetFirst();
                    Transicion tra3 = new Transicion(epsilon, n4);
                    n1.Transiciones.AddLast(tra3);
                    n5 = Estructura2.GetLast();
                    n6 = new Nodo(n++);
                    Transicion tra5 = new Transicion(epsilon, n6);
                    n3.Transiciones.AddLast(tra5);
                    n5.Transiciones.AddLast(tra5);
                }
            }
            this.SetFirst(n1);
            this.SetLast(n6);
            return null;
        }
    }
    public class And : Estructura
    {
        Estructura Estructura1, Estructura2;
        Nodo n1, n2, n3;
        public And(Estructura estructura1, Estructura estructura2)
        {
            Estructura1 = estructura1;
            Estructura2 = estructura2;
        }
        public override object Ejecutar(int n)
        {
            return null;
        }

        public override object Numerar(ref int n)
        {
            if (Estructura1 is Terminal term1)
            {
                //.ab
                if (Estructura2 is Terminal term2)
                {
                    n1 = new Nodo(n++);
                    n2 = new Nodo(n++);
                    Transicion tra1 = new Transicion(term1, n2);
                    n1.Transiciones.AddLast(tra1);
                    n3 = new Nodo(n++);
                    Transicion tra2 = new Transicion(term2, n3);
                    n2.Transiciones.AddLast(tra2);
                    this.SetFirst(n1);
                    this.SetLast(n3);
                }
                //.a.bc
                else
                {
                    n1 = new Nodo(n++);
                    Estructura2.Numerar(ref n);
                    n2 = Estructura2.GetFirst();
                    Transicion tra1 = new Transicion(term1, n2);
                    n1.Transiciones.AddLast(tra1);
                    this.SetFirst(n1);
                    this.SetLast(Estructura2.GetLast());
                }
            }
            else
            {
                Estructura1.Numerar(ref n);
                n2 = Estructura1.GetLast();
                //..abc
                if (Estructura2 is Terminal term2)
                {
                    n3 = new Nodo(n++);
                    Transicion tra = new Transicion(term2, n3);
                    n2.Transiciones.AddLast(tra);
                    this.SetFirst(Estructura1.GetFirst());
                    this.SetLast(n3);
                }
                //..ab.cd
                else
                {
                    Estructura2.Numerar(ref n);
                    Terminal epsilon = new Terminal(Terminal.Tipo.EPSILON, "");
                    n3 = Estructura2.GetFirst();
                    Transicion tra = new Transicion(epsilon, n3);
                    n2.Transiciones.AddLast(tra);
                    this.SetFirst(Estructura1.GetFirst());
                    this.SetLast(Estructura2.GetLast());
                }
            }
            return null;
        }
    }
    public class Kleen : Estructura
    {
        Estructura Estructura1;
        Nodo n1, n2, n3, n4;
        public Kleen(Estructura estructura1)
        {
            Estructura1 = estructura1;
        }
        public override object Ejecutar(int n)
        {
            return null;
        }

        public override object Numerar(ref int n)
        {
            Terminal epsilon = new Terminal(Terminal.Tipo.EPSILON, "");
            if (Estructura1 is Terminal term)
            {
                n1 = new Nodo(n++);
                n2 = new Nodo(n++);
                Transicion tra1 = new Transicion(epsilon, n2);
                n1.Transiciones.AddLast(tra1);
                n3 = new Nodo(n++);
                Transicion tra2 = new Transicion(term, n3);
                n2.Transiciones.AddLast(tra2);
                Transicion tra3 = new Transicion(epsilon, n2);
                n3.Transiciones.AddLast(tra3);
                n4 = new Nodo(n++);
                Transicion tra4 = new Transicion(epsilon, n4);
                n3.Transiciones.AddLast(tra4);
                //Transicion tra5 = new Transicion(epsilon, n4); n1.Transiciones.AddLast(tra5);
                n1.Transiciones.AddLast(tra4);
                this.SetFirst(n1);
                this.SetLast(n4);
            }
            else
            {
                n1 = new Nodo(n++);
                Estructura1.Numerar(ref n);
                n2 = Estructura1.GetFirst();
                Transicion tra1 = new Transicion(epsilon, n2);
                n1.Transiciones.AddLast(tra1);
                n3 = Estructura1.GetLast();
                Transicion tra2 = new Transicion(epsilon, n2);
                n3.Transiciones.AddLast(tra2);
                n4 = new Nodo(n++);
                Transicion tra4 = new Transicion(epsilon, n4);
                n3.Transiciones.AddLast(tra4);
                //Transicion tra5 = new Transicion(epsilon, n4); n1.Transiciones.AddLast(tra5);
                n1.Transiciones.AddLast(tra4);
                this.SetFirst(n1);
                this.SetLast(n4);
            }
            return null;
        }
    }
    public class Terminal : Estructura
    {
        public enum Tipo
        {
            ID,
            CADENA,
            CARACTER_ESPECIAL,
            C_TODO,
            EPSILON
        }
        Terminal.Tipo TipoTerminal;
        string Valor;
        public Terminal(Terminal.Tipo tipo, string val)
        {
            TipoTerminal = tipo;
            Valor = val;
        }
        public string GetValor()
        {
            return Valor;
        }
        public Terminal.Tipo GetTipoTerminal()
        {
            return TipoTerminal;
        }
        public override object Ejecutar(int n)
        {
            return null;
        }
        public override object Numerar(ref int n)
        {
            return null;
        }
    }
    public class Nodo
    {
        public int Numero;
        public LinkedList<Transicion> Transiciones;
        public Nodo(int num)
        {
            Numero = num;
            Transiciones = new LinkedList<Transicion>();
        }

    }
    public class Transicion
    {
        Terminal Terminal;
        Nodo NodoDestino;
        public Transicion(Terminal term, Nodo nodoDestino)
        {
            Terminal = term;
            NodoDestino = nodoDestino;
        }
    }
}
