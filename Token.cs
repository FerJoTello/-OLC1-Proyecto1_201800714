using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201800714
{
    public class Token
    {
        public enum Tipo
        {
            //ELEMENTOS
            ID,
            LETRA,  //Minus y mayus.
            CADENA,
            C_TODO, //Caracter especial [:todo:] (funciona como una cadena).
            CARACTER_ESPECIAL,  //Salto de linea, tab, comilla simple y comilla doble.
            NUMERO,
            SIGNO,
            //PALABRAS RESERVADAS
            PR_CONJ,
            //SIMBOLO
            S_DOS_PUNTOS,
            S_COMA,
            S_PUNTO_Y_COMA,
            S_FLECHA,
            S_PUNTO,
            S_VIRGUILLA,
            S_PLECA,
            S_INTERROGACION,
            S_ASTERISCO,
            S_SUMA,
            S_LLAVE_IZQ,
            S_LLAVE_DER,
            //COMENTARIOS
            COMENTARIO_INLINE,
            COMENTARIO_MULTILINE, 
            INDEFINIDO
        }
        private Tipo tipoToken;
        private String valor;
        private int fila;
        private int columna;
        public Token(Tipo tipoToken, String valor, int fila, int columna)
        {
            this.tipoToken = tipoToken;
            this.valor = valor;
            this.fila = fila;
            this.columna = columna;
        }
        public String GetValor()
        {
            return this.valor;
        }

        public Token.Tipo GetTipo()
        {
            return this.tipoToken;

        }
        public int GetFila()
        {
            return this.fila;
        }
        public int GetColumna()
        {
            return this.columna;
        }
    }
}
