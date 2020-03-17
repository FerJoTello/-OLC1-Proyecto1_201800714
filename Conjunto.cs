using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201800714
{
    class Conjunto
    {
        String Nombre;
        LinkedList<string> Elementos;

        public Conjunto(string nombre, LinkedList<string> elementos)
        {
            Nombre = nombre;
            Elementos = elementos;
        }
        public String GetNombre()
        {
            return this.Nombre;
        }
        public LinkedList<string> GetElementos()
        {
            return this.Elementos;
        }
    }
}
