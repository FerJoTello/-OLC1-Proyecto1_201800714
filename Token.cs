using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201800714
{
    class Token
    {
        public enum Tipo
        {
            //ELEMENTOS
            ID,
            LETRA,
            CADENA,
            CARACTER,
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
            //CARACTERES ESPECIALES
            C_SALTO,
            C_COMILLA_SIMPLE,
            C_COMILLA_DOBLE,
            C_TAB,
            C_TODO,
            COMENTARIO_INLINE,
            COMENTARIO_MULTILINE, //solo un token para comentario :v, y los especiales los tengo como valor conjunto :v
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
