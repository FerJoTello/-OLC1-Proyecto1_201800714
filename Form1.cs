using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _OLC1_Proyecto1_201800714
{
    public partial class Form1 : Form
    {
        //Contador de pestañas
        private int cont_pestaña = 1;
        private LinkedList<string> ListaImagenes = new LinkedList<string>();
        private int imagen_actual = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void AbrirArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "C:\\Users\\Fernando\\OneDrive\\Documentos";
            ofd.Filter = "Archivos ER (*.er)|*.er|Todos los Archivos (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //Verificando existencia de archivo
                try
                {
                    string ruta = ofd.FileName;
                    var flujo = ofd.OpenFile();
                    string contenido;
                    using (StreamReader lector = new StreamReader(flujo))
                    {
                        contenido = lector.ReadToEnd();
                    }
                    bool existe = tabControl1.SelectedTab.HasChildren;
                    if (existe)
                    {
                        tabControl1.SelectedTab.Controls.RemoveAt(0);
                        RichTextBox nuevaCajaTexto = new RichTextBox();

                        nuevaCajaTexto.Dock = DockStyle.Fill;
                        nuevaCajaTexto.Text = contenido;
                        tabControl1.SelectedTab.Controls.Add(nuevaCajaTexto);
                        tabControl1.SelectedTab.Controls.SetChildIndex(nuevaCajaTexto, 0);
                    }
                    else
                    {
                        RichTextBox nuevaCajaTexto = new RichTextBox();
                        nuevaCajaTexto.Dock = DockStyle.Fill;
                        nuevaCajaTexto.Text = contenido;
                        tabControl1.SelectedTab.Controls.Add(nuevaCajaTexto);
                    }
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("Ha ocurrido un error en la carga.");
                }
            }
        }
        private void guardarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Control item in tabControl1.SelectedTab.Controls)
            {
                Boolean a = typeof(RichTextBox).IsInstanceOfType(item);
                if (a)
                {
                    try
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "Archivos ER | *.er";
                        saveFileDialog.ShowDialog();
                        String ruta = saveFileDialog.FileName;
                        String contenido = item.Text;
                        File.WriteAllText(ruta, contenido);
                    }
                    catch (ArgumentException)
                    {

                    }
                }
            }
        }


        private void agregarPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cont_pestaña++;
            TabPage pestañaN = new CustomTab(cont_pestaña);
            tabControl1.TabPages.Add(pestañaN);
        }
        //Boton Analizar Entrada
        private void button1_Click(object sender, EventArgs e)
        {
            String entrada;
            foreach (Control item in this.tabControl1.SelectedTab.Controls)
            {
                Boolean a = typeof(RichTextBox).IsInstanceOfType(item);
                if (a)
                {
                    //Recuperando texto del RichTextBox de la pestaña seleccionada para el analisis.
                    entrada = item.Text;
                    entrada += "#";
                    AnalizadorLexico lexico = new AnalizadorLexico();
                    LinkedList<Token> listaTokens = lexico.Analizar(entrada);
                    richTextBox1.Text = "**Inicia Análisis Léxico**\n";
                    richTextBox1.Text += lexico.imprimirListaToken(listaTokens);
                    richTextBox1.Text += "**Finaliza Análisis Léxico**\n";
                    AnalizadorER analizadorER = new AnalizadorER();
                    richTextBox1.Text += "**Inicia Análisis Sintáctico**\n";
                    if (listaTokens.Count > 0)
                    {
                        analizadorER.Parsear(listaTokens);
                        if (analizadorER.RutasImagenes.Count > 0)
                        {
                            ListaImagenes = analizadorER.RutasImagenes;
                            pictureBox1.Visible = true;
                            pictureBox1.Image = Image.FromFile(ListaImagenes.First.Value);
                            button2.Enabled = false;
                            button3.Enabled = false;
                            if (ListaImagenes.Count > 1)
                            {
                                button3.Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        richTextBox1.Text += "El analisis lexico no devolvió ningún token. No se iniciar análsis sintáctico";
                    }
                    if (!analizadorER.existenciaError)
                    {
                        richTextBox1.Text += analizadorER.consola;
                        richTextBox1.Text += "*SIN ERRORES SINTACTICOS*\n**Finaliza Análisis Sintáctico**\n";
                    }
                    else
                    {
                        richTextBox1.Text += analizadorER.consola;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(ListaImagenes.ElementAt(--imagen_actual));
            if (imagen_actual == 0)
            {
                button2.Enabled = false;
                button3.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(ListaImagenes.ElementAt(++imagen_actual));
            button2.Enabled = true;
            if (imagen_actual == ListaImagenes.Count - 1)
            {
                button3.Enabled = false;
            }
        }
    }
    public class CustomTab : TabPage
    {
        private int num;
        public CustomTab(int contarPestaña)
        {
            this.num = contarPestaña;
            RichTextBox cajadetexto = new RichTextBox();
            this.Controls.Add(cajadetexto);
            this.Text = "Pestaña " + num;
            cajadetexto.Dock = DockStyle.Fill;
        }
    }
}
