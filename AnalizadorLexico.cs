using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201800714
{
    class AnalizadorLexico
    {
        //Variable que representa la lista de tokens
        private LinkedList<Token> salida;
        //Variable que representa el estado actual
        private int estado;
        //Variable que representa el lexema que actualmente se esta acumulando
        private String auxlex;
        private int fila = 1;
        private int columna = 1;
        //Metodo que se encarga de analizar la entrada.
        public LinkedList<Token> Analizar(String entrada)
        {
            //Valor de return
            salida = new LinkedList<Token>();
            estado = 0;
            auxlex = "";
            char c;
            Console.WriteLine("******************************************");
            for (int i = 0; i < entrada.Length; i++)
            {
                c = entrada.ElementAt(i);
                //Cada caso representa un estado del analizador léxico y es usado para determinar qué es cada lexema.
                //Si dentro de cada caso se cumple una condicion se agrega el caracter analizado a una cadena que sirve para guardar el lexema continuamente.
                //Sino se cumple ninguna se procede a indicar un error léxico.
                switch (estado)
                {
                    //Estado Inicial (todos los demás son estados de aceptación)
                    case 0:
                        if (Char.IsLetter(c))
                        {
                            agregarCaracter(c);
                            //Compara los caracteres anteriores y posteriores para saber si es una letra perteneciente a un conjunto.
                            //Si el siguiente o el anterior caracter es igual a '~' o ','
                            char cNext = entrada.ElementAt(i + 1);
                            char cPrev = entrada.ElementAt(i - 1);
                            if (cNext.CompareTo('~') == 0 || cNext.CompareTo(',') == 0)
                            {
                                agregarToken(Token.Tipo.LETRA);
                            }
                            else if (cPrev.CompareTo('~') == 0 || cPrev.CompareTo(',') == 0)
                            {
                                agregarToken(Token.Tipo.LETRA);
                            }
                            //Estado de ID y Pal Reserv
                            else
                            {
                                estado = 1;
                            }
                        }
                        else if (Char.IsDigit(c))
                        {
                            agregarCaracter(c);
                            //Estado de Numero
                            estado = 4;
                        }
                        else if (c.CompareTo('-') == 0)
                        {
                            agregarCaracter(c);
                            //Para flecha
                            char cNext = entrada.ElementAt(i + 1);
                            //Revisa si el siguiente es el simbolo '>' para poder validar la flecha
                            if (cNext.CompareTo('>') == 0)
                            {
                                agregarCaracter(cNext);
                                estado = 2;
                                //Como ya se analizó el siguiente ('>') entonces se procede a analizar el posterior.
                                i += 1;
                            }
                            else
                            {
                                //Error, '-' no está definido en el lenguaje.
                                agregarToken(Token.Tipo.INDEFINIDO);
                            }
                        }
                        else if (c.CompareTo(':') == 0)
                        {
                            agregarCaracter(c);
                            estado = 3;
                        }
                        else if (c.CompareTo(';') == 0)
                        {
                            agregarCaracter(c);
                            estado = 5;
                        }
                        else if (c.CompareTo('~') == 0)
                        {
                            agregarCaracter(c);
                            estado = 6;
                        }
                        else if (c.CompareTo(',') == 0)
                        {
                            agregarCaracter(c);
                            estado = 7;
                        }
                        else if (c.CompareTo('{') == 0)
                        {
                            agregarCaracter(c);
                            estado = 8;
                        }
                        else if (c.CompareTo('}') == 0)
                        {
                            agregarCaracter(c);
                            estado = 9;
                        }
                        //operadores
                        else if (c.CompareTo('.') == 0)
                        {
                            agregarCaracter(c);
                            estado = 10;
                        }
                        else if (c.CompareTo('|') == 0)
                        {
                            agregarCaracter(c);
                            estado = 11;
                        }
                        else if (c.CompareTo('?') == 0)
                        {
                            agregarCaracter(c);
                            estado = 12;
                        }
                        else if (c.CompareTo('*') == 0)
                        {
                            agregarCaracter(c);
                            estado = 13;
                        }
                        else if (c.CompareTo('+') == 0)
                        {
                            agregarCaracter(c);
                            estado = 14;
                        }
                        //Especial [:todo:]
                        else if (c.CompareTo('[') == 0)
                        {
                            agregarCaracter(c);
                            char a = entrada.ElementAt(i + 1);
                            if (a.CompareTo(':') == 0)
                            {
                                estado = 23;
                                i++;
                                agregarCaracter(a);
                            }
                            else
                            {
                                agregarToken(Token.Tipo.INDEFINIDO);
                            }
                        }
                        //caracter ESPECIAL
                        else if (c.CompareTo('\\') == 0)
                        {
                            agregarCaracter(c);
                            estado = 20;
                        }
                        //cadena
                        else if (c.CompareTo('"') == 0)
                        {
                            agregarCaracter(c);
                            estado = 16;
                        }
                        //signo
                        else if (Encoding.ASCII.GetBytes(c.ToString())[0] >= 33 && Encoding.ASCII.GetBytes(c.ToString())[0] <= 38)
                        {
                            agregarCaracter(c);
                            estado = 17;
                        }
                        //comentarios inline
                        else if (c.CompareTo('/') == 0)
                        {
                            agregarCaracter(c);
                            char a = entrada.ElementAt(i + 1);
                            if (a.CompareTo('/') == 0)
                            {
                                estado = 18;
                                i++;
                                agregarCaracter(a);
                            }
                            else
                            {
                                agregarToken(Token.Tipo.INDEFINIDO);
                            }
                        }
                        //comentarios multiline
                        else if (c.CompareTo('<') == 0)
                        {
                            agregarCaracter(c);
                            char a = entrada.ElementAt(i + 1);
                            if (a.CompareTo('!') == 0)
                            {
                                estado = 22;
                                i++;
                                agregarCaracter(a);
                            }
                            else
                            {
                                agregarToken(Token.Tipo.INDEFINIDO);
                            }
                        }
                        else
                        {
                            if (c.CompareTo(' ') == 0)
                            {
                                columna++;
                            }
                            else if (c.CompareTo('\n') == 0)
                            {
                                fila++;
                                columna = 1;
                            }
                            else if (c.CompareTo('#') == 0 && i == entrada.Length - 1)
                            {
                                //Hemos concluido el análisis léxico.
                                Console.WriteLine("******************************************");
                            }
                            else
                            {
                                agregarCaracter(c);
                                agregarToken(Token.Tipo.INDEFINIDO);
                            }
                        }
                        break;
                    //Estado en el que recibe una letra
                    case 1:
                        //Se sigue aceptando si vienen letra o digito o guion bajo.
                        if (Char.IsLetterOrDigit(c) || c.CompareTo('_') == 0)
                        {
                            agregarCaracter(c);
                        }
                        else
                        {
                            //Reservada "CONJ"
                            if (auxlex.Equals("CONJ"))
                            {
                                agregarToken(Token.Tipo.PR_CONJ);
                            }
                            //ID
                            else
                            {
                                agregarToken(Token.Tipo.ID);
                            }
                            //Como el caracter no forma parte de la expresion entonces se vuelve a analizar
                            i -= 1;
                        }
                        break;
                    case 2:
                        //Para Flecha
                        if (auxlex.CompareTo("->") == 0)
                        {
                            agregarToken(Token.Tipo.S_FLECHA);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        i -= 1;
                        break;
                    //Dos puntos
                    case 3:
                        if (auxlex.CompareTo(":") == 0)
                        {
                            agregarToken(Token.Tipo.S_DOS_PUNTOS);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        i -= 1;
                        break;
                    //Estado de Numero
                    case 4:
                        //Si el caracter es otro digito se agrega al lexema auxiliar.
                        if (Char.IsDigit(c))
                        {
                            agregarCaracter(c);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.NUMERO);
                            //Regresa a analizar un caracter anterior.
                            i -= 1;
                        }
                        break;
                    //Punto y coma
                    case 5:
                        if (auxlex.CompareTo(";") == 0)
                        {
                            agregarToken(Token.Tipo.S_PUNTO_Y_COMA);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        i -= 1;
                        break;
                    //Virguilla
                    case 6:
                        if (auxlex.CompareTo("~") == 0)
                        {
                            agregarToken(Token.Tipo.S_VIRGUILLA);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        i -= 1;
                        break;
                    //Coma
                    case 7:
                        if (auxlex.CompareTo(",") == 0)
                        {
                            agregarToken(Token.Tipo.S_COMA);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        i -= 1;
                        break;
                    //Llave Izquierda
                    case 8:
                        if (auxlex.CompareTo("{") == 0)
                        {
                            agregarToken(Token.Tipo.S_LLAVE_IZQ);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        i -= 1;
                        break;
                    //Llave Derecha
                    case 9:
                        if (auxlex.CompareTo("}") == 0)
                        {
                            agregarToken(Token.Tipo.S_LLAVE_DER);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        i -= 1;
                        break;
                    //Punto
                    case 10:
                        if (auxlex.CompareTo(".") == 0)
                        {
                            agregarToken(Token.Tipo.S_PUNTO);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        i -= 1;
                        break;
                    //Pleca
                    case 11:
                        if (auxlex.CompareTo("|") == 0)
                        {
                            agregarToken(Token.Tipo.S_PLECA);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        i -= 1;
                        break;
                    //Interrogacion
                    case 12:
                        if (auxlex.CompareTo("?") == 0)
                        {
                            agregarToken(Token.Tipo.S_INTERROGACION);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        i -= 1;
                        break;
                    //Asterisco
                    case 13:
                        if (auxlex.CompareTo("*") == 0)
                        {
                            agregarToken(Token.Tipo.S_ASTERISCO);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        i -= 1;
                        break;
                    //Suma
                    case 14:
                        if (auxlex.CompareTo("+") == 0)
                        {
                            agregarToken(Token.Tipo.S_SUMA);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        i -= 1;
                        break;
                    //Cadena
                    case 16:
                        if (c.CompareTo('"') != 0 && i != entrada.Length - 1)
                        {
                            agregarCaracter(c);
                            if (c.CompareTo('\n') == 0)
                            {
                                fila++;
                                columna = 1;
                            }
                        }
                        else if (c.CompareTo('"') == 0)
                        {
                            estado = 19;
                            agregarCaracter(c);
                        }
                        else if (c.CompareTo('#') == 0 && i == entrada.Length - 1)
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                            i--;
                        }
                        break;
                    //Signo
                    case 17:
                        if (auxlex.Length == 1)
                        {
                            agregarToken(Token.Tipo.SIGNO);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        i -= 1;
                        break;
                    //Comentario Inline
                    case 18:
                        if (c.CompareTo('\n') != 0)
                        {
                            agregarCaracter(c);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.COMENTARIO_INLINE);
                            fila++;
                            columna = 1;
                        }
                        break;
                    //Validacion de Cadena
                    case 19:
                        char v = '"';
                        if (auxlex.StartsWith(v.ToString()) && auxlex.EndsWith(v.ToString()))
                        {
                            agregarToken(Token.Tipo.CADENA);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        i -= 1;
                        break;
                    //Caracter especial
                    case 20:
                        if (c.CompareTo('n') == 0 || c.CompareTo('\'') == 0 || c.CompareTo('\"') == 0 || c.CompareTo('t') == 0)
                        {
                            agregarCaracter(c);
                            estado = 21;
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                            i--;
                        }
                        break;
                    //Validacion de caracter especial
                    case 21:
                        if (auxlex.StartsWith("\\") && auxlex.Length == 2)
                        {
                            agregarToken(Token.Tipo.CARACTER_ESPECIAL);
                        }
                        else
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        i -= 1;
                        break;
                    //Comentario Multiline
                    case 22:
                        if (c.CompareTo('!') == 0)
                        {
                            agregarCaracter(c);
                            char a = entrada.ElementAt(i + 1);
                            if (a.CompareTo('>') == 0)
                            {
                                i++;
                                agregarCaracter(a);
                                agregarToken(Token.Tipo.COMENTARIO_MULTILINE);
                            }
                        }
                        else
                        {
                            if (c.CompareTo('#') == 0 && i == entrada.Length - 1)
                            {
                                agregarToken(Token.Tipo.INDEFINIDO);
                                i--;
                            }
                            else
                            {
                                agregarCaracter(c);
                                if (c.CompareTo('\n') == 0)
                                {
                                    fila++;
                                    columna = 1;
                                }
                            }
                        }
                        break;
                    //[:todo:]
                    case 23:
                        if (c.CompareTo(':') != 0 && c.CompareTo('\n') != 0 && i != entrada.Length - 1)
                        {
                            agregarCaracter(c);
                        }
                        else if (c.CompareTo(':') == 0)
                        {
                            agregarCaracter(c);
                            char a = entrada.ElementAt(i + 1);
                            if (a.CompareTo(']') == 0)
                            {
                                i++;
                                agregarCaracter(a);
                                agregarToken(Token.Tipo.C_TODO);
                            }
                        }
                        else if (c.CompareTo('\n') == 0)
                        {
                            i--;
                            agregarToken(Token.Tipo.INDEFINIDO);
                        }
                        else if (c.CompareTo('#') == 0 && i == entrada.Length - 1)
                        {
                            agregarToken(Token.Tipo.INDEFINIDO);
                            i--;
                        }
                        break;
                }
            }
            return salida;
        }
        //Metodo que permite agregar un nuevo token a la lista de la salida del AnalizadorLexico y regresa al estado inicial para seguir analizando
        public void agregarToken(Token.Tipo tipo)
        {
            salida.AddLast(new Token(tipo, auxlex, fila, columna));
            auxlex = "";
            estado = 0;
        }
        public void agregarCaracter(char c)
        {
            auxlex += c;
            columna++;
        }
        public string imprimirListaToken(LinkedList<Token> lista)
        {
            int cont = 0;
            string consola = "| NO. | LEXEMA | TIPO | FILA | COLUMNA |\n";
            foreach (Token item in lista)
            {
                consola += "| " + cont + " | " + item.GetValor() + " | " + item.GetTipo() + " | " + item.GetFila() + " | " + item.GetColumna() + " | \n";
                cont++;
            }
            return consola;
        }
    }
}
