using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201800714
{
    class AnalizadorER
    {
        /**
            controlToken: indica el indice de la lista de tokens con el que se esta trabajando.
            tokenActual: variable de la lista que se obtiene a partir de controlToken.
            listaTokens: tokens del analisis lexico.
            tablaSimbolos: objeto donde se guardan los valores de las variables encontradas.
            existenciaError: booleano que indica si se encontro un error Sintactico en todo el analisis.
            errorSintactico: bandera booleana que sirve para indicar a este objeto si existe un error (ver Parea).
            consola: texto que se genera para mostrar al usuario sobre los posibles errores sintacticos.
        */
        int controlToken;
        Token tokenActual;
        LinkedList<Token> listaTokens;
        public Dictionary<string, Simbolo> tablaSimbolos = new Dictionary<string, Simbolo>();
        public string consola;
        public bool existenciaError = false;
        bool errorSintactico = false;
        /*
        <INICIO>::=<LISTA_INSTRUCCIONES>
        <LISTA_INSTRUCCIONES>::=<DECLARACION_CONJ> <LISTA_INSTRUCCIONES> 							
                            |	ID <LISTA_INSTRUCCIONES'>								
                            |	epsilon
        <LISTA_INSTRUCCIONES'>::=<DECLARACION_ER> <LISTA_INSTRUCCIONES>
                            |	<VALIDACION_ER> <LISTA_INSTRUCCIONES>				
        <DECLARACION_CONJ>::= 	PR_CONJ S_DOS_PUNTOS ID S_FLECHA <ELEMENTO> <RANGO> S_PUNTO_Y_COMA	
        <ELEMENTO>::=			LETRA
                            |	NUMERO
                            |	SIGNO
                            |	CARACTER_ESPECIAL
        <RANGO>::=				S_VIRGUILLA	<ELEMENTO>
                            |	<RANGO_P>
        <RANGO_P>::=			S_COMA <ELEMENTO> <RANGO_P>
                            |	epsilon
        <DECLARACION_ER>::=		S_FLECHA <ESTRUCTURA> S_PUNTO_Y_COMA
        <ESTRUCTURA>::=			S_PUNTO <ESTRUCTURA'> <ESTRUCTURA'>
                            |	S_PLECA	<ESTRUCTURA'> <ESTRUCTURA'>
                            |	S_INTERROGACION	<ESTRUCTURA'>
                            |	S_ASTERISCO	<ESTRUCTURA'>
                            |	S_SUMA <ESTRUCTURA'>
        <ESTRUCTURA'>::=		S_LLAVE_IZQ ID S_LLAVE_DER
                            |	CADENA
                            |	CARACTER_ESPECIAL
                            |	C_TODO
                            |	<ESTRUCTURA>
        <VALIDACION_ER>::=		S_DOS_PUNTOS CADENA S_PUNTO_Y_COMA
        */
        //primero: hacer el parea. con una bandera booleana que indique si hay error sintactico
        //segundo: durante el parea. crear una tabla de simbolos para conjuntos y las ER
        //tercero: bajo la misma estructura de la gramatica se llamara recursivamente a los elementos de la ER para poder generar sus AFND y AFD para luego validarlos.
        /* Comparador:
         * Verifica que el tokenActual sea del mismo tipo que se solicita. (Se usa en 'Primeros' y con ello se elige una alternativa dependiendo la produccion de la gramatica)
         */
        private Boolean Comparador(Token.Tipo tipo)
        {
            return tokenActual.GetTipo() == tipo;
        }




        /**
        *  Parea:
        *  Compara si el token de preanalisis tiene el tipo que se indica, en caso de que no sean iguales marca error.
        **/
        public void Parea(Token.Tipo tipoToken)
        {
            //Si existe un error sintactico buscara al simbolo punto y coma para poder continuar con el analisis.
            if (errorSintactico)
            {
                if (controlToken < listaTokens.Count - 1)
                {
                    controlToken++;
                    tokenActual = listaTokens.ElementAt(controlToken);
                    if (tokenActual.GetTipo() == Token.Tipo.S_PUNTO_Y_COMA)
                    {
                        errorSintactico = false;
                    }
                    else
                    {
                        Console.WriteLine("Ya no se pudo recuperar :c");
                    }
                }
            }
            //Si no hay error previo se procede a comparar si cumple con Parea.
            else
            {
                //Parea se cumple bien
                if (tokenActual.GetTipo() == tipoToken)
                {
                    if (controlToken < listaTokens.Count - 1)
                    {
                        controlToken++;
                        tokenActual = listaTokens.ElementAt(controlToken);
                        while ((tokenActual.GetTipo() == Token.Tipo.COMENTARIO_INLINE || tokenActual.GetTipo() == Token.Tipo.COMENTARIO_MULTILINE) && controlToken < listaTokens.Count - 1)
                        {
                            controlToken++;
                            tokenActual = listaTokens.ElementAt(controlToken);
                        }
                    }
                }
                //Error sintactico
                else
                {
                    Console.WriteLine("Error Sintactico\nEn ID_Token: " + controlToken + "\nSe esperaba [" + tipoToken.ToString() + "] en lugar de [" + tokenActual.GetTipo() + ", " + tokenActual.GetValor() + "]");
                    consola += "*Error Sintactico*\nEn ID_Token: " + controlToken + "\nSe esperaba [" + tipoToken.ToString() + "] en lugar de [" + tokenActual.GetTipo() + ", " + tokenActual.GetValor() + "]\n";
                    errorSintactico = true;
                    existenciaError = true;
                }
            }
        }
        /**
        *  Parsear:
        *  Inicia el analisis sintactico llamando al no terminal inicio.
        **/
        public void Parsear(LinkedList<Token> tokens)
        {
            this.listaTokens = tokens;
            controlToken = 0;
            tokenActual = listaTokens.ElementAt(controlToken);
            Inicio();
        }
        /**
         * TODOS LOS SIGUIENTES SON NO TERMINALES PERTENECIENTES A LA GRAMATICA DEL ANALISIS SINTACTICO.
         * Si en un No terminal no se cumple ningun caso de los esperados se marca un error sintactico (generalmente se maneja en los else; a menos que venga un epsilon, entonces no se hace nada).
         */
        /**
         * Inicio:
         * No terminal que funciona como simbolo inicial para la gramatica del analisis sintactico.
         */
        public void Inicio()
        {
            //Por si entra un comentario como primer token, pasan al parea y los omite.
            //Si no fuera un comentario el primer token procede a la ListaInstrucciones().
            if (Comparador(Token.Tipo.COMENTARIO_INLINE))
            {
                Parea(Token.Tipo.COMENTARIO_INLINE);
            }
            else if (Comparador(Token.Tipo.COMENTARIO_MULTILINE))
            {
                Parea(Token.Tipo.COMENTARIO_MULTILINE);
            }
            ListaInstrucciones();
        }
        public void ListaInstrucciones()
        {
            if (Comparador(Token.Tipo.PR_CONJ))
            {
                Declaracion_CONJ();
                ListaInstrucciones();
            }
            else if (Comparador(Token.Tipo.ID))
            {
                ListaInstrucciones_P();
            }
            else
            {
                //epsilon
            }
        }
        /**
         * Declaracion_CONJ espera una declaracion para un conjunto nuevo.
         * Por ello se recupera el valor del token ID y el valor de los elementos en una lista para luego incluirlos en la tabla de simbolos como una nueva variable.
         */
        public void Declaracion_CONJ()
        {
            Parea(Token.Tipo.PR_CONJ);
            Parea(Token.Tipo.S_DOS_PUNTOS);
            string id = tokenActual.GetValor();
            Parea(Token.Tipo.ID);
            Parea(Token.Tipo.S_FLECHA);
            LinkedList<string> elementos = new LinkedList<string>();
            try
            {
                Token primero = tokenActual;
                elementos.AddLast(Elemento());
                //Se envia parametro de la lista elementos por referencia al metodo Rango().
                Rango(primero, ref elementos);
            }
            catch (ArgumentNullException)
            {
                //Error sintactico. Se maneja dentro de Elemento() y Rango(elementos)
            }
            Parea(Token.Tipo.S_PUNTO_Y_COMA);
            //Se crea un nuevo objeto conjunto
            Conjunto conjunto = new Conjunto(id, elementos);
            //Se vincula el objeto conjunto a un objeto simbolo que se agrega a la tablaSimbolos
            Simbolo simbolo = new Simbolo(id, "Conjunto", conjunto);
            //Es agregado el nuevo simbolo con su 'key' que es utilizado para evitar variables con el mismo nombre
            try
            {
                tablaSimbolos.Add(id, simbolo);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Ya existe un objeto con ID = " + id);
            }
        }
        /**
         * Elemento devuelve un string que contiene el valor del token de un elemento del conjunto y hace Parea dependiendo el tipo.
         */
        public string Elemento()
        {
            string elemento = tokenActual.GetValor();
            if (Comparador(Token.Tipo.LETRA))
            {
                Parea(Token.Tipo.LETRA);
            }
            else if (Comparador(Token.Tipo.NUMERO))
            {
                Parea(Token.Tipo.NUMERO);
            }
            else if (Comparador(Token.Tipo.SIGNO))
            {
                Parea(Token.Tipo.SIGNO);
            }
            else if (Comparador(Token.Tipo.CARACTER_ESPECIAL))
            {
                Parea(Token.Tipo.CARACTER_ESPECIAL);
            }
            else
            {
                Console.WriteLine("Error Sintactico\nEn ID_Token: " + controlToken + "\nSe esperaba [LETRA|NUMERO|SIGNO|CARACTER_ESPECIAL] en lugar de [" + tokenActual.GetTipo() + ", " + tokenActual.GetValor() + "]");
                consola += "*Error Sintactico*\nEn ID_Token: " + controlToken + "\nSe esperaba [LETRA|NUMERO|SIGNO|CARACTER_ESPECIAL] en lugar de [" + tokenActual.GetTipo() + ", " + tokenActual.GetValor() + "]\n";
                errorSintactico = true;
                existenciaError = true;
                return null;
            }
            return elemento;
        }
        //Recibe parametro por referencia, lo que indica que el objeto que se recibe es exactamente el mismo que se envia.
        //Por lo que al modificarlo dentro del metodo sufrira los cambios y permaneceran incluso en el ambito desde el cual fue llamado.
        public void Rango(Token primero, ref LinkedList<string> elementos)
        {
            if (Comparador(Token.Tipo.S_VIRGUILLA))
            {
                Parea(Token.Tipo.S_VIRGUILLA);
                Token ultimo = tokenActual;
                //Si se manejan numeros como elementos
                if (primero.GetTipo() == Token.Tipo.NUMERO && ultimo.GetTipo() == Token.Tipo.NUMERO)
                {
                    int first = int.Parse(primero.GetValor());
                    int last = int.Parse(ultimo.GetValor());
                    for (int i = first + 1; i < last; i++)
                    {
                        elementos.AddLast(i.ToString());
                    }
                }
                //si no son numeros
                else
                {
                    byte[] firstAscii = Encoding.ASCII.GetBytes(primero.GetValor());
                    byte[] lastAscii = Encoding.ASCII.GetBytes(ultimo.GetValor());
                    for (int i = firstAscii.ElementAt(0) + 1; i < lastAscii.ElementAt(0); i++)
                    {
                        ///char niu = (char)i; string niuS = ((char)i).ToString(); elementos.AddLast(niuS);
                        elementos.AddLast(((char)i).ToString());
                    }
                }
                elementos.AddLast(Elemento());
            }
            else if (Comparador(Token.Tipo.S_COMA))
            {
                Rango_P(ref elementos);
            }
            else
            {
                Console.WriteLine("Error Sintactico\nEn ID_Token: " + controlToken + "\nSe esperaba [S_VIRGUILLA|S_COMA] en lugar de [" + tokenActual.GetTipo() + ", " + tokenActual.GetValor() + "]");
                consola += "*Error Sintactico*\nEn ID_Token: " + controlToken + "\nSe esperaba [S_VIRGUILLA|S_COMA] en lugar de [" + tokenActual.GetTipo() + ", " + tokenActual.GetValor() + "]\n";
                errorSintactico = true;
                existenciaError = true;
            }
        }
        public void Rango_P(ref LinkedList<string> elementos)
        {
            if (Comparador(Token.Tipo.S_COMA))
            {
                Parea(Token.Tipo.S_COMA);
                elementos.AddLast(Elemento());
                Rango_P(ref elementos);
            }
            else
            {
                //epsilon
            }
        }
        public void ListaInstrucciones_P()
        {
            string id = tokenActual.GetValor();
            Parea(Token.Tipo.ID);
            if (Comparador(Token.Tipo.S_FLECHA))
            {
                Declaracion_ER(id);
                ListaInstrucciones();
            }
            else if (Comparador(Token.Tipo.S_DOS_PUNTOS))
            {
                Validacion_ER(id);
                ListaInstrucciones();
            }
            else
            {
                Console.WriteLine("Error Sintactico\nEn ID_Token: " + controlToken + "\nSe esperaba [S_FLECHA|S_DOS_PUNTOS] en lugar de [" + tokenActual.GetTipo() + ", " + tokenActual.GetValor() + "]");
                consola += "*Error Sintactico*\nEn ID_Token: " + controlToken + "\nSe esperaba [S_FLECHA|S_DOS_PUNTOS] en lugar de [" + tokenActual.GetTipo() + ", " + tokenActual.GetValor() + "]\n";
                errorSintactico = true;
                existenciaError = true;
            }
        }
        /* Declaracion_ER:  Guarda en la tabla de simbolos un objeto de tipo Estructura.
         */
        public void Declaracion_ER(string id)
        {
            Parea(Token.Tipo.S_FLECHA);
            try
            {
                //Se crea una ListaTerminales que se va llenando conforme se encuentra un terminal en Estructura_P.
                //Se crea una ListaTerminales que contiene las transiciones con el valor real para que se pueda alcanzar cierto estado.
                LinkedList<Terminal> ListaTerminales = new LinkedList<Terminal>();
                //Se crea un ListaNodos que se va llenando conforme se realiza el AFN.
                LinkedList<Nodo> ListaNodos = new LinkedList<Nodo>();
                //Se crea un nuevo objeto estructura.
                Estructura estructura = Estructura(ref ListaTerminales);
                Parea(Token.Tipo.S_PUNTO_Y_COMA);
                /***************************************************** AFN *****************************************************/
                int n = 0;
                //Creacion de nodos y sus transiciones.
                estructura.Numerar(ref ListaNodos, ref n);
                //Creacion de grafica.
                String CadenaGraphviz = "digraph AFN{\n" +
                    "\trankdir=LR;\n" +
                    "\tGraph[label = \"AFN: " + id + "\"];\n" +
                    "\tnode [shape = circle, fontsize = 10; style = filled fillcolor=gray, color = black, fontcolor = black];\n" +
                    "\tI [fontsize = 1; style = filled fillcolor=white,  fontcolor = white, color = white];\n" +
                    "\tI->n" + estructura.GetFirst().Numero + "[label = Io];\n";
                //Graficar nodos.
                Nodo First = estructura.GetFirst();
                First.Graficar(ref CadenaGraphviz);
                CadenaGraphviz += "\tn" + estructura.GetLast().Numero + "[shape = doublecircle];\n" +
                    "}";
                Graficador graficadorAFN = new Graficador("AFN_" + id);
                //Creacion de archivos.
                graficadorAFN.graficar(CadenaGraphviz);
                /***************************************************** AFD *****************************************************/
                int contadorCerraduras = 0;
                //A
                LinkedList<Nodo> c0 = new LinkedList<Nodo>(First.ObtenerCerraduras().OrderBy(nodo => nodo.Numero));
                Cerradura Cerradura0 = new Cerradura(c0);
                Cerradura0.Estado = "n" + contadorCerraduras++;
                LinkedList<Cerradura> Cerraduras = new LinkedList<Cerradura>();
                Cerraduras.AddLast(Cerradura0);
                bool nuevaEntrada;
                //Creacion de Estados y sus transiciones.
                do
                {
                    Console.WriteLine("Sexo");
                    nuevaEntrada = false;
                    //Dictionary<string, Cerradura> auxiliar = new Dictionary<string, Cerradura>(Cerraduras);
                    LinkedList<Cerradura> auxiliar = new LinkedList<Cerradura>(Cerraduras);
                    foreach (Cerradura cerradura in Cerraduras)
                    {
                        if (!cerradura.Evaluado)
                        {
                            //a (terminal con el que se trabaja Mover).
                            foreach (Terminal terminal in ListaTerminales)
                            {
                                //M(A,a) (mover).
                                LinkedList<Nodo> Mover = new LinkedList<Nodo>();
                                foreach (Nodo elementoCerradura in cerradura.Elementos)
                                {
                                    elementoCerradura.ObtenerMover(ref Mover, terminal.GetRepresentacion());
                                }
                                if (Mover.Count != 0)
                                {
                                    //C(M(A,a)) (cerradura del mover)
                                    LinkedList<Nodo> cm = new LinkedList<Nodo>();
                                    foreach (Nodo elementoMover in Mover)
                                    {
                                        cm = new LinkedList<Nodo>(cm.Union(elementoMover.ObtenerCerraduras()));
                                    }
                                    cm = new LinkedList<Nodo>(cm.OrderBy(nodo => nodo.Numero));
                                    bool existe = false;
                                    //Recorre el diccionario revisando si ya existe un estado igual para agregar la nueva transición.
                                    int tamano = cm.Count;
                                    foreach (Cerradura estado in auxiliar)
                                    {
                                        bool iguales = false;
                                        if (estado.Elementos.Count == cm.Count)
                                        {
                                            iguales = true;
                                            for (int i = 0; i < tamano; i++)
                                            {
                                                if (estado.Elementos.ElementAt(i).Numero != cm.ElementAt(i).Numero)
                                                {
                                                    existe = false;
                                                    iguales = false;
                                                    break;
                                                }
                                            }
                                            if (iguales)
                                            {
                                                TransicionC transicion = new TransicionC(terminal, estado);
                                                cerradura.ListaTransiciones.AddLast(transicion);
                                                existe = true;
                                                break;
                                            }
                                        }
                                    }
                                    //Si no existe ese estado, se crea y se agrega a la lista de cerraduras auxiliar.
                                    if (!existe)
                                    {
                                        Cerradura CerraduraMover = new Cerradura(cm);
                                        CerraduraMover.Estado = "n" + contadorCerraduras++;
                                        auxiliar.AddLast(CerraduraMover);
                                        TransicionC transicion = new TransicionC(terminal, CerraduraMover);
                                        cerradura.ListaTransiciones.AddLast(transicion);
                                        nuevaEntrada = true;
                                    }
                                }
                            }
                            cerradura.Evaluado = true;
                        }
                    }
                    Cerraduras = auxiliar;
                } while (nuevaEntrada);
                CadenaGraphviz = "digraph AFD{\n" +
                    "\trankdir=LR;\n" +
                    "\tGraph[label = \"AFD: " + id + "\" fontcolor = dodgerblue4];\n" +
                    "\tnode [shape = circle, fontsize = 10; colorscheme = pubu9, style = filled, fillcolor = 5, color = 6, fontcolor = 1];\n" +
                    "\tI [fontsize = 1; style = filled fillcolor=white,  fontcolor = white, color = white];\n" +
                    "\tI->n0[label = Io, colorscheme = pubu9, color = 9, fontcolor = 9];\n";
                foreach (Cerradura estado in Cerraduras)
                {
                    if (estado.Elementos.Contains(estructura.GetLast()))
                    {
                        estado.Aceptacion = true;
                    }
                    estado.Graficar(ref CadenaGraphviz);
                }
                CadenaGraphviz += "}";
                Graficador graficadorAFD = new Graficador("AFD_" + id);
                //Creacion de archivos.
                graficadorAFD.graficar(CadenaGraphviz);
                //Se vincula el objeto Cerradura a un objeto simbolo que se agrega a la tablaSimbolos.
                Simbolo simbolo = new Simbolo(id, "Estado", Cerraduras.ElementAt(0));
                //Es agregado el nuevo simbolo con su 'key' que es utilizado para evitar variables con el mismo nombre.
                try
                {
                    tablaSimbolos.Add(id, simbolo);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Ya existe un objeto con ID = " + id);
                }
            }
            catch (NullReferenceException)
            {
                //Error sintactico. Manejado en Estructura.
            }
        }
        /*  Estructura: Devuelve un objeto de tipo Estructura dependiendo el tipo de Estructura que se espera trabajar.
         *              Puede llegarse a llamar recursivamente si llama al metodo Estructura_P ya que quiere decir que 
         *              espera a otra Estructura que proviene de un operador (ver gramatica).
         */
        public Estructura Estructura(ref LinkedList<Terminal> terminals)
        {
            if (Comparador(Token.Tipo.S_PUNTO))
            {
                Parea(Token.Tipo.S_PUNTO);
                And and = new And(Estructura_P(ref terminals), Estructura_P(ref terminals));
                return and;
            }
            else if (Comparador(Token.Tipo.S_PLECA))
            {
                Parea(Token.Tipo.S_PLECA);
                Or or = new Or(Estructura_P(ref terminals), Estructura_P(ref terminals));
                return or;
            }
            else if (Comparador(Token.Tipo.S_INTERROGACION))
            {
                Parea(Token.Tipo.S_INTERROGACION);
                Terminal epsilon = new Terminal(Terminal.Tipo.EPSILON, "ε");
                Or interrogacion = new Or(Estructura_P(ref terminals), epsilon);
                return interrogacion;
            }
            else if (Comparador(Token.Tipo.S_ASTERISCO))
            {
                Parea(Token.Tipo.S_ASTERISCO);
                Kleen kleen = new Kleen(Estructura_P(ref terminals));
                return kleen;
            }
            else if (Comparador(Token.Tipo.S_SUMA))
            {
                Parea(Token.Tipo.S_SUMA);
                Estructura estructura = Estructura_P(ref terminals);
                Kleen kleen = new Kleen(estructura);
                And suma = new And(estructura, kleen);
                return suma;
            }
            else
            {
                Console.WriteLine("Error Sintactico\nEn ID_Token: " + controlToken + "\nSe esperaba [S_PUNTO|S_PLECA|S_INTERROGACION|S_ASTERISCO|S_SUMA] en lugar de [" + tokenActual.GetTipo() + ", " + tokenActual.GetValor() + "]");
                consola += "*Error Sintactico*\nEn ID_Token: " + controlToken + "\nSe esperaba [S_PUNTO|S_PLECA|S_INTERROGACION|S_ASTERISCO|S_SUMA] en lugar de [" + tokenActual.GetTipo() + ", " + tokenActual.GetValor() + "]\n";
                errorSintactico = true;
                existenciaError = true;
                return null;
            }
        }
        /*  Estructura_P:
         * 
         */
        public Estructura Estructura_P(ref LinkedList<Terminal> terminals)
        {
            if (Comparador(Token.Tipo.S_LLAVE_IZQ))
            {
                Parea(Token.Tipo.S_LLAVE_IZQ);
                Terminal id = new Terminal(Terminal.Tipo.ID, tokenActual.GetValor());
                bool existe = this.ComprobarExistenciaEnLista(terminals, id);
                if (!existe)
                {
                    try
                    {   //Revisa en la tabla de simbolos si existe un conjunto con el id solicitado y añade los elementos del conjunto a su lista de valores.
                        Simbolo simbolo = tablaSimbolos[tokenActual.GetValor()];
                        Conjunto conjunto = (Conjunto)simbolo.GetValor();
                        id.ListaValores = new LinkedList<string>(conjunto.GetElementos());
                        terminals.AddLast(id);
                    }
                    catch (KeyNotFoundException)
                    {
                        //No se pudo obtener el conjunto de la tabla de simbolos, asi que no se guarda el Terminal en la lista de Terminales para la validacion.
                    }
                }
                Parea(Token.Tipo.ID);
                Parea(Token.Tipo.S_LLAVE_DER);
                return id;
            }
            else if (Comparador(Token.Tipo.CADENA))
            {
                Terminal cadena = new Terminal(Terminal.Tipo.CADENA, tokenActual.GetValor());
                bool existe = this.ComprobarExistenciaEnLista(terminals, cadena);
                if (!existe)
                {
                    terminals.AddLast(cadena);
                }
                Parea(Token.Tipo.CADENA);
                return cadena;
            }
            else if (Comparador(Token.Tipo.CARACTER_ESPECIAL))
            {
                Terminal c_especial = new Terminal(Terminal.Tipo.CARACTER_ESPECIAL, tokenActual.GetValor());
                bool existe = this.ComprobarExistenciaEnLista(terminals, c_especial);
                if (!existe)
                {
                    terminals.AddLast(c_especial);
                }
                Parea(Token.Tipo.CARACTER_ESPECIAL);
                return c_especial;
            }
            else if (Comparador(Token.Tipo.C_TODO))
            {
                Terminal c_todo = new Terminal(Terminal.Tipo.C_TODO, tokenActual.GetValor());
                bool existe = this.ComprobarExistenciaEnLista(terminals, c_todo);
                if (!existe)
                {
                    terminals.AddLast(c_todo);
                }
                Parea(Token.Tipo.C_TODO);
                return c_todo;
            }
            else if (Comparador(Token.Tipo.S_PUNTO) || Comparador(Token.Tipo.S_PLECA) || Comparador(Token.Tipo.S_INTERROGACION) || Comparador(Token.Tipo.S_ASTERISCO) || Comparador(Token.Tipo.S_SUMA))
            {
                return Estructura(ref terminals);
            }
            else
            {
                Console.WriteLine("Error Sintactico\nEn ID_Token: " + controlToken + "\nSe esperaba [S_LLAVE_IZQ|CADENA|CARACTER_ESPECIAL|C_TODO|S_PUNTO|S_PLECA|S_INTERROGACION|S_ASTERISCO|S_SUMA] en lugar de [" + tokenActual.GetTipo() + ", " + tokenActual.GetValor() + "]");
                consola += "*Error Sintactico*\nEn ID_Token: " + controlToken + "\nSe esperaba [S_LLAVE_IZQ|CADENA|CARACTER_ESPECIAL|C_TODO|S_PUNTO|S_PLECA|S_INTERROGACION|S_ASTERISCO|S_SUMA] en lugar de [" + tokenActual.GetTipo() + ", " + tokenActual.GetValor() + "]\n";
                errorSintactico = true;
                existenciaError = true;
                return null;
            }
        }
        /*  ComprobarExistenciaEnLista: Método que devuelve un booleano indicando si un terminal pertenece a la lista de terminales.
         */
        private Boolean ComprobarExistenciaEnLista(LinkedList<Terminal> terminals, Terminal term)
        {
            foreach (Terminal terminal in terminals)
            {
                if (terminal.GetValorReal().Equals(term.GetValorReal()))
                {
                    return true;
                }
            }
            return false;
        }

        public void Validacion_ER(string id)
        {
            Parea(Token.Tipo.S_DOS_PUNTOS);
            string lexema = tokenActual.GetValor();
            Parea(Token.Tipo.CADENA);
            Parea(Token.Tipo.S_PUNTO_Y_COMA);
            //Aqui continua la validacion del lexema.
            consola += "**Evaluando Expresion Regular: " + id + "**\n";
            try
            {
                Simbolo simbolo = tablaSimbolos[id];    //Posible Exception.
                if (!simbolo.GetTipo().Equals("Estado"))
                {
                    throw new KeyNotFoundException();   //Como no es compatible el tipo entonces no se puede trabajar con él. Se manda Exception.
                }
                Cerradura estado = (Cerradura)simbolo.GetValor();

            }
            catch (KeyNotFoundException)
            {
                consola += "No se pudo validar lexema, no se encontró su expresión regular\n**Fin evaluacion**\n";
            }
        }
    }
}
