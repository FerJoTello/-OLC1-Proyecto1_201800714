using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201800714
{
    class Simbolo
    {
        private String Tipo;
        private String Nombre;
        private Object Valor;
        public Simbolo(string nombre, string tipo, object valor)
        {
            Tipo = tipo;
            Nombre = nombre;
            Valor = valor;
        }
        public String GetTipo()
        {
            return this.Tipo;
        }
        public String GetNombre()
        {
            return this.Nombre;
        }
        public Object GetValor()
        {
            return this.Valor;
        }
    }
}
