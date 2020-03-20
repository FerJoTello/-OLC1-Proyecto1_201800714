using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201800714
{   /* Estructura:      Clase abstracta que permite analizar una ER al recuperar la info en el Analisis Sintactico.
     *                  Esto con el fin de controlar a cada Operador (or, and y kleen) e indicar su estructura sin ser especifico en qué es lo que se espera.
     *                  Por lo que cada Operador puede obtener otro Operador (or, and y kleen) o un terminal y así cumplir con la estructura que solicita cada uno.
     * Parametros:  ->  First: Nodo (estado) inicial de la estructura. Se genera dependiendo el tipo (or, and y kleen)
     *              ->  Last: Nodo final de la estructura (estado de aceptacion).
     * Metodos:     ->  Ejecutar(int n): utilizado para generar acciones.
     *              ->  Numerar(int n): utilizado para enumerar cada nodo y así generar el AFN.
     *                  Para la numeracion se suele llamar recursivamente dependiendo el tipo de la Estructura guardando los nodos FIrst y Last ya que con ellos se enlazan 
     *                  los nodos de una Estructura con otra.
     */
    public abstract class Estructura
    {
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
        public abstract object Numerar(ref LinkedList<Nodo> listaNodos, ref int n);
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

        public override object Numerar(ref LinkedList<Nodo> listaNodos, ref int n)
        {
            Terminal epsilon = new Terminal(Terminal.Tipo.EPSILON, "ε");
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
                    Transicion tra6 = new Transicion(epsilon, n6);
                    n5.Transiciones.AddLast(tra6);
                    listaNodos.AddLast(n1);
                    listaNodos.AddLast(n2);
                    listaNodos.AddLast(n3);
                    listaNodos.AddLast(n4);
                    listaNodos.AddLast(n5);
                    listaNodos.AddLast(n6);

                }
                else
                {
                    Estructura2.Numerar(ref listaNodos, ref n);
                    n4 = Estructura2.GetFirst();
                    Transicion tra3 = new Transicion(epsilon, n4);
                    n1.Transiciones.AddLast(tra3);
                    n5 = Estructura2.GetLast();
                    n6 = new Nodo(n++);
                    Transicion tra5 = new Transicion(epsilon, n6);
                    n3.Transiciones.AddLast(tra5);
                    Transicion tra6 = new Transicion(epsilon, n6);
                    n5.Transiciones.AddLast(tra6);
                    listaNodos.AddLast(n1);
                    listaNodos.AddLast(n2);
                    listaNodos.AddLast(n3);
                    listaNodos.AddLast(n6);
                }
            }
            else
            {
                n1 = new Nodo(n++);
                Estructura1.Numerar(ref listaNodos, ref n);
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
                    Transicion tra6 = new Transicion(epsilon, n6);
                    n5.Transiciones.AddLast(tra6);
                    listaNodos.AddLast(n1);
                    listaNodos.AddLast(n4);
                    listaNodos.AddLast(n5);
                    listaNodos.AddLast(n6);
                }
                else
                {
                    Estructura2.Numerar(ref listaNodos, ref n);
                    n4 = Estructura2.GetFirst();
                    Transicion tra3 = new Transicion(epsilon, n4);
                    n1.Transiciones.AddLast(tra3);
                    n5 = Estructura2.GetLast();
                    n6 = new Nodo(n++);
                    Transicion tra5 = new Transicion(epsilon, n6);
                    n3.Transiciones.AddLast(tra5);
                    Transicion tra6 = new Transicion(epsilon, n6);
                    n5.Transiciones.AddLast(tra6);
                    listaNodos.AddLast(n1);
                    listaNodos.AddLast(n6);
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

        public override object Numerar(ref LinkedList<Nodo> listaNodos, ref int n)
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
                    listaNodos.AddLast(n1);
                    listaNodos.AddLast(n2);
                    listaNodos.AddLast(n3);
                }
                //.a.bc
                else
                {
                    n1 = new Nodo(n++);
                    Estructura2.Numerar(ref listaNodos, ref n);
                    n2 = Estructura2.GetFirst();
                    Transicion tra1 = new Transicion(term1, n2);
                    n1.Transiciones.AddLast(tra1);
                    this.SetFirst(n1);
                    this.SetLast(Estructura2.GetLast());
                    listaNodos.AddLast(n1);
                }
            }
            else
            {
                Estructura1.Numerar(ref listaNodos, ref n);
                n2 = Estructura1.GetLast();
                //..abc
                if (Estructura2 is Terminal term2)
                {
                    n3 = new Nodo(n++);
                    Transicion tra = new Transicion(term2, n3);
                    n2.Transiciones.AddLast(tra);
                    this.SetFirst(Estructura1.GetFirst());
                    this.SetLast(n3);
                    listaNodos.AddLast(n3);
                }
                //..ab.cd
                else
                {
                    Estructura2.Numerar(ref listaNodos, ref n);
                    Terminal epsilon = new Terminal(Terminal.Tipo.EPSILON, "ε");
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

        public override object Numerar(ref LinkedList<Nodo> listaNodos, ref int n)
        {
            Terminal epsilon = new Terminal(Terminal.Tipo.EPSILON, "ε");
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
                Transicion tra5 = new Transicion(epsilon, n4);
                n1.Transiciones.AddLast(tra5);
                this.SetFirst(n1);
                this.SetLast(n4);
                listaNodos.AddLast(n1);
                listaNodos.AddLast(n2);
                listaNodos.AddLast(n3);
                listaNodos.AddLast(n4);
            }
            else
            {
                n1 = new Nodo(n++);
                Estructura1.Numerar(ref listaNodos, ref n);
                n2 = Estructura1.GetFirst();
                Transicion tra1 = new Transicion(epsilon, n2);
                n1.Transiciones.AddLast(tra1);
                n3 = Estructura1.GetLast();
                Transicion tra2 = new Transicion(epsilon, n2);
                n3.Transiciones.AddLast(tra2);
                n4 = new Nodo(n++);
                Transicion tra4 = new Transicion(epsilon, n4);
                n3.Transiciones.AddLast(tra4);
                Transicion tra5 = new Transicion(epsilon, n4);
                n1.Transiciones.AddLast(tra5);
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
            if (tipo == Terminal.Tipo.CADENA)
            {
                Valor = Valor.Insert(0, "\\");
                Valor = Valor.Insert(Valor.Length - 1, "\\");
            }
            else if (tipo == Terminal.Tipo.CARACTER_ESPECIAL)
            {
                Valor = Valor.Insert(0, "\\");
                if (Valor.Equals("\\\\\""))
                {
                    Valor = Valor.Insert(0, "\\");
                }
            }
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
        public override object Numerar(ref LinkedList<Nodo> listaNodos, ref int n)
        {
            return null;
        }
    }
    public class Nodo
    {
        public int Numero;
        public LinkedList<Transicion> Transiciones;
        public LinkedList<Nodo> Cerradura;
        public bool ObtuvoCerradura;
        public Nodo(int num)
        {
            Numero = num;
            Transiciones = new LinkedList<Transicion>();
            Cerradura = new LinkedList<Nodo>();
            Cerradura.AddLast(this);
            ObtuvoCerradura = false;
        }
        public void Graficar(ref string cadenaGraphviz)
        {
            foreach (Transicion transicion in Transiciones)
            {
                if (!transicion.Graficado)
                {
                    Terminal terminal = transicion.Terminal;
                    Nodo nodoDestino = transicion.NodoDestino;
                    if (terminal.GetTipoTerminal() == Terminal.Tipo.EPSILON)
                    {
                        this.Cerradura.AddLast(nodoDestino);
                    }
                    cadenaGraphviz += "\tn" + Numero + "->n" + nodoDestino.Numero + "[label = \"" + terminal.GetValor() + "\"];\n";
                    transicion.Graficado = true;
                    transicion.NodoDestino.Graficar(ref cadenaGraphviz);
                }
            }
        }
        public LinkedList<Nodo> ObtenerCerraduras()
        {
            LinkedList<Nodo> auxiliar = new LinkedList<Nodo>(this.Cerradura);
            if (!this.ObtuvoCerradura)
            {
                foreach (Nodo cerradurita1 in this.Cerradura)
                {
                    if (!cerradurita1.Equals(this))
                    {
                        foreach (Nodo cerradurita2 in cerradurita1.Cerradura)
                        {
                            if (!cerradurita2.Equals(cerradurita1) && cerradurita1.Cerradura.Count > 1)
                            {
                                if (!auxiliar.Contains(cerradurita2))
                                {
                                    if (cerradurita2.Cerradura.Count == 1)
                                    {
                                        auxiliar.AddLast(cerradurita2);
                                    }
                                    //Es un estado que posee mas transiciones epsilon.
                                    else
                                    {
                                        //Obtiene sus cerraduras ySe las agrega a la listaAuxiliar.
                                        foreach (Nodo final in cerradurita2.ObtenerCerraduras())
                                        {
                                            if (!auxiliar.Contains(final))
                                            {
                                                auxiliar.AddLast(final);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            this.Cerradura = auxiliar;
            this.ObtuvoCerradura = true;
            return this.Cerradura;
            /*
            for (int i = 0; i < this.Cerradura.Count; i++)
            {
                Nodo nodoCerradura = this.Cerradura.ElementAt(i);
                for (int j = 0; j < nodoCerradura.Cerradura.Count; i++)
                {
                    Nodo cerradurita = nodoCerradura.Cerradura.ElementAt(j);
                    if (!this.Cerradura.Contains(cerradurita))
                    {
                        this.Cerradura.AddLast(cerradurita);
                    }
                }
            }
            */
        }
        public void ObtenerMover(ref LinkedList<Nodo> mover, String terminal)
        {
            foreach (Transicion transicion in this.Transiciones)
            {
                if (transicion.Terminal.GetValor().Equals(terminal))
                {
                    mover.AddLast(transicion.NodoDestino);
                }
            }
        }
    }
    public class Transicion
    {
        public Terminal Terminal;
        public Nodo NodoDestino;
        public Boolean Graficado;
        public Transicion(Terminal term, Nodo nodoDestino)
        {
            Terminal = term;
            NodoDestino = nodoDestino;
            Graficado = false;
        }
    }
}
