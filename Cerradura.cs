using iTextSharp.text.pdf;
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
     *  Atributos:  ->  Estado:             Nombre del estado del AFD.   
     *              ->  Elementos:          Lista de nodos que forman el conjunto de cerradura.
     *              ->  Evaluado:           Bandera booleana que indica si el objeto ya pasó por las iteraciones del Método de Thompson. 
     *              ->  ListaTransiciones:  Contiene las transiciones que hacen llegar a este Estado.
     */
    class Cerradura
    {
        public String Estado;
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
        public void Graficar(ref string cadenaGraphviz)
        {
            foreach (TransicionC transicion in ListaTransiciones)
            {
                if (!transicion.Graficado)
                {
                    string idNext = transicion.EstadoSiguiente.Estado;
                    string terminal = transicion.Terminal.GetRepresentacion();
                    cadenaGraphviz += "\t" + this.Estado + "->" + idNext + "[label = \"" + terminal + "\", colorscheme = pubu9, color = 9, fontcolor = 9];\n";
                }
            }
            if (Aceptacion)
            {
                cadenaGraphviz += "\t" + this.Estado + "[shape = doublecircle];\n";
            }
        }
        public void Tablear(ref PdfPTable tabla, LinkedList<Terminal> ListaTerminales)
        {
            foreach (Terminal terminal in ListaTerminales)
            {
                bool exito = false;
                foreach (TransicionC transicion in this.ListaTransiciones)
                {
                    if (transicion.Terminal.GetRepresentacion().Equals(terminal.GetRepresentacion()))
                    {
                        tabla.AddCell(transicion.EstadoSiguiente.Estado);
                        exito = true;
                    }
                }
                if (!exito)
                {
                    tabla.AddCell("");
                }
            }
        }
        public bool Validar(string lexema, ref int contador, ref int filaIncio, ref int columnaInicio, ref LinkedList<Token> tokens)
        {
            if (contador < lexema.Length)
            {
                foreach (TransicionC transicion in this.ListaTransiciones)
                {

                    if (transicion.Terminal.GetTipoTerminal() == Terminal.Tipo.ID)
                    {
                        foreach (string elemento in transicion.Terminal.ListaValores)
                        {
                            string auxiliar = elemento;
                            Token.Tipo tipo = Token.Tipo.INDEFINIDO;
                            try
                            {
                                if (auxiliar.Equals("\\t"))
                                {
                                    auxiliar = "\t";
                                    tipo = Token.Tipo.CARACTER_ESPECIAL;
                                }
                                else if (auxiliar.Equals("\\n"))
                                {
                                    auxiliar = "\n";
                                    tipo = Token.Tipo.CARACTER_ESPECIAL;
                                }
                                else if (auxiliar.Equals("\\'"))
                                {
                                    auxiliar = "\'";
                                    tipo = Token.Tipo.CARACTER_ESPECIAL;
                                }
                                else if (auxiliar.Equals("\\\""))
                                {
                                    auxiliar = "\"";
                                    tipo = Token.Tipo.CARACTER_ESPECIAL;
                                }
                                else if (auxiliar.Length == 1 && Char.IsLetter(auxiliar.ElementAt(0)))
                                {
                                    tipo = Token.Tipo.LETRA;
                                }
                                else if (Char.IsDigit(auxiliar.ElementAt(0)))
                                {
                                    tipo = Token.Tipo.NUMERO;
                                }
                                else if (Encoding.ASCII.GetBytes(auxiliar.ElementAt(0).ToString())[0] >= 33 && Encoding.ASCII.GetBytes(auxiliar.ElementAt(0).ToString())[0] <= 38)
                                {
                                    tipo = Token.Tipo.SIGNO;
                                }
                                else if (auxiliar.StartsWith("[:") && auxiliar.EndsWith(":]"))
                                {
                                    auxiliar = auxiliar.Remove(0, 2);
                                    auxiliar = auxiliar.Remove(auxiliar.Length - 2, 2);
                                    tipo = Token.Tipo.C_TODO;
                                }
                                //Si equivale el auxiliar en la cadena.
                                if (auxiliar.Equals(lexema.Substring(contador, auxiliar.Length)))
                                {
                                    Token token = new Token(tipo, elemento, filaIncio, columnaInicio);
                                    tokens.AddLast(token);
                                    columnaInicio += auxiliar.Length;
                                    if (auxiliar.Equals("\n"))
                                    {
                                        filaIncio++;
                                        columnaInicio = 1;
                                    }
                                    contador += auxiliar.Length;
                                    return transicion.EstadoSiguiente.Validar(lexema, ref contador, ref filaIncio, ref columnaInicio, ref tokens);
                                }
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                //No hace nada ya que si lanza esta exception quiere decir que simplemente no es una transicion valida.
                                //Así que prueba con otro.
                            }
                        }
                    }
                    else if (transicion.Terminal.GetTipoTerminal() == Terminal.Tipo.CADENA)
                    {
                        string elemento = transicion.Terminal.GetValorReal();
                        try
                        {
                            elemento = elemento.Remove(0, 1);
                            elemento = elemento.Remove(elemento.Length - 1, 1);
                            if (elemento.Equals(lexema.Substring(contador, elemento.Length)))
                            {
                                Token token = new Token(Token.Tipo.CADENA, transicion.Terminal.GetValorReal(), filaIncio, columnaInicio);
                                tokens.AddLast(token);
                                columnaInicio += elemento.Length;
                                contador += elemento.Length;
                                return transicion.EstadoSiguiente.Validar(lexema, ref contador, ref filaIncio, ref columnaInicio, ref tokens);
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            //No hace nada ya que si lanza esta exception quiere decir que simplemente no es una transicion valida.
                            //Así que prueba con otro.
                        }
                    }
                    else if (transicion.Terminal.GetTipoTerminal() == Terminal.Tipo.CARACTER_ESPECIAL)
                    {
                        string elemento = "";
                        try
                        {
                            if (transicion.Terminal.GetValorReal().Equals("\\t"))
                            {
                                elemento = "\t";
                            }
                            else if (transicion.Terminal.GetValorReal().Equals("\\n"))
                            {
                                elemento = "\n";
                            }
                            else if (transicion.Terminal.GetValorReal().Equals("\\'"))
                            {
                                elemento = "\'";
                            }
                            else if (transicion.Terminal.GetValorReal().Equals("\\\""))
                            {
                                elemento = "\"";
                            }
                            if (elemento.Equals(lexema.Substring(contador, elemento.Length)))
                            {
                                Token token = new Token(Token.Tipo.CARACTER_ESPECIAL, transicion.Terminal.GetValorReal(), filaIncio, columnaInicio);
                                tokens.AddLast(token);
                                columnaInicio += elemento.Length;
                                if (elemento.Equals("\n"))
                                {
                                    filaIncio++;
                                    columnaInicio = 1;
                                }
                                contador += elemento.Length;
                                return transicion.EstadoSiguiente.Validar(lexema, ref contador, ref filaIncio, ref columnaInicio, ref tokens);
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            //No hace nada ya que si lanza esta exception quiere decir que simplemente no es una transicion valida.
                            //Así que prueba con otro.
                        }
                    }
                    else if (transicion.Terminal.GetTipoTerminal() == Terminal.Tipo.C_TODO)
                    {
                        string elemento = transicion.Terminal.GetValorReal();
                        try
                        {
                            elemento = elemento.Remove(0, 2);
                            elemento = elemento.Remove(elemento.Length - 2, 2);
                            if (elemento.Equals(lexema.Substring(contador, elemento.Length)))
                            {
                                Token token = new Token(Token.Tipo.C_TODO, transicion.Terminal.GetValorReal(), filaIncio, columnaInicio);
                                tokens.AddLast(token);
                                columnaInicio += elemento.Length;
                                contador += elemento.Length;
                                return transicion.EstadoSiguiente.Validar(lexema, ref contador, ref filaIncio, ref columnaInicio, ref tokens);
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            //No hace nada ya que si lanza esta exception quiere decir que simplemente no es una transicion valida.
                            //Así que prueba con otro.
                        }
                    }
                }
                Token tokenF = new Token(Token.Tipo.INDEFINIDO, lexema.Substring(contador), filaIncio, columnaInicio);
                tokens.AddLast(tokenF);
                Console.WriteLine("Error en la validación. El lexema no es valido según, su expresión regular.");
                return false;
            }
            else if (contador == lexema.Length)
            {
                return Aceptacion;
            }
            else
            {
                //si el contador es mayor al lexema.
                Console.WriteLine("Algo pasó, no debería de entrar aquí");
                return false;
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
        public Terminal Terminal;
        public Cerradura EstadoSiguiente;
        public Boolean Graficado;
        public TransicionC(Terminal terminal, Cerradura next)
        {
            Terminal = terminal;
            EstadoSiguiente = next;
            Graficado = false;
        }

    }
}
