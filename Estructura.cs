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
        public abstract object Ejecutar(int n);
        public abstract object Numerar(int n);
    }
    public class Or : Estructura
    {
        Estructura Estructura1, Estructura2;
        int n1, n2, n3, n4, n5, n6;
        public Or(Estructura estructura1, Estructura estructura2)
        {
            Estructura1 = estructura1;
            Estructura2 = estructura2;
        }
        public override object Ejecutar(int n)
        {
            return null;
        }

        public override object Numerar(int n)
        {
            return null;
        }
    }
    public class And : Estructura
    {
        Estructura Estructura1, Estructura2;
        int n1, n2, n3;
        public And(Estructura estructura1, Estructura estructura2)
        {
            Estructura1 = estructura1;
            Estructura2 = estructura2;
        }
        public override object Ejecutar(int n)
        {
            return null;
        }

        public override object Numerar(int n)
        {
            return null;
        }
    }
    public class Kleen : Estructura
    {
        Estructura Estructura1;
        int n1, n2, n3, n4;
        public Kleen(Estructura estructura1)
        {
            Estructura1 = estructura1;
        }
        public override object Ejecutar(int n)
        {
            return null;
        }

        public override object Numerar(int n)
        {
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
        public override object Ejecutar(int n)
        {
            return null;
        }

        public override object Numerar(int n)
        {
            return null;
        }
    }
    public class TipoTerminal
    {
        
    }
}
