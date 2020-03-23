using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _OLC1_Proyecto1_201800714
{
    class Graficador
    {
        String ruta;
        String nombre;
        StringBuilder grafo;
        public Graficador(string id)
        {
            nombre = id;
            ruta = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            string carpeta = ruta + "\\Graphviz";
            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }
            ruta = carpeta;
        }
        private void generarDot(String rdot, String rpng)
        {
            File.WriteAllText(rdot, grafo.ToString());
            String comandoDot = "dot.exe -Tpng " + rdot + " -o " + rpng + " ";
            var comando = string.Format(comandoDot);
            var procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/C" + comando);
            var proc = new System.Diagnostics.Process();
            proc.StartInfo = procStart;
            proc.Start();
            proc.WaitForExit();
        }
        public String graficar(String texto)
        {
            grafo = new StringBuilder();
            String rdot = ruta + "\\" + nombre + ".dot";
            String rpng = ruta + "\\" + nombre + ".png";
            grafo.Append(texto);
            this.generarDot(rdot, rpng);
            return rpng;
        }
    }
}
