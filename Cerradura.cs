using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201800714
{
    /*  Cerradura:  Objeto que se almacena en un Dictionary con el fin de llevar un mejor control de los conjuntos Cerradura.
     *              Posteriormente se utiliza como un Estado (nodo gráfico) del AFD.
     *  
     *  Atributos   ->  Elementos:          Lista de nodos que forman el conjunto de cerradura.
     *              ->  Evaluado:           Bandera booleana que indica si el objeto ya pasó por las iteraciones del Método de Thompson. 
     *              ->  ListaTransiciones:  Contiene las transiciones que hacen llegar a este Estado.
     */
    class Cerradura
    {
        public LinkedList<Nodo> Elementos;
        public Boolean Evaluado, Aceptacion;
        public LinkedList<TransicionC> ListaTransiciones;
        public Cerradura(LinkedList<Nodo> elementos)
        {
            Elementos = elementos;
            Evaluado = false;
            Aceptacion = false;
            ListaTransiciones = new LinkedList<TransicionC>();
        }
        public void Graficar(string idNodo, ref string cadenaGraphviz)
        {
            foreach (TransicionC transicion in ListaTransiciones)
            {
                if (!transicion.Graficado)
                {
                    string idPrevio = transicion.EstadoPrevio;
                    string terminal = transicion.Terminal;
                    cadenaGraphviz += "\t" + idPrevio + "->" + idNodo + "[label = \"" + terminal + "\", colorscheme = pubu9, color = 9, fontcolor = 9];\n";
                    if (Aceptacion)
                    {
                        cadenaGraphviz += "\t" + idNodo + "[shape = doublecircle];\n";
                    }

                }
            }
        }
    }
    /*  TransicionC:    Objeto utilizado para indicar las transiciones que contiene un estado guardando un terminal y su Estado previo.
     * 
     *  Atributos:      ->  Terminal:       Objeto con el que realiza la transicion.
     *                  ->  EstadoPrevio:   Guarda el nombre del estado del que provino.
     */
    class TransicionC
    {
        public String Terminal;
        public String EstadoPrevio;
        public Boolean Graficado;
        public TransicionC(String terminal, String previo)
        {
            Terminal = terminal;
            EstadoPrevio = previo;
            Graficado = false;
        }

    }
}
