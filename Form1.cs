using iTextSharp.text;
using iTextSharp.text.pdf;
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
        private int cont_pestaña = 1;
        private LinkedList<string> ListaImagenes = new LinkedList<string>();
        private LinkedList<Token> ListaTokens = new LinkedList<Token>();
        private LinkedList<Token> ListaErrores = new LinkedList<Token>();
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
            ListaImagenes = new LinkedList<string>();
            pictureBox1.Image = null;
            pictureBox1.Visible = false;
            button2.Enabled = false;
            button3.Enabled = false;
            imagen_actual = -1;
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
                    //Si la lista de tokens del léxico no está vacía...
                    if (listaTokens.Count > 0)
                    {
                        //Si no hay error en el análisis de la entrada procede a realizar el sintáctico.
                        if (!lexico.existenciaError)
                        {
                            richTextBox1.Text += "**Inicia Análisis Sintáctico**\n";
                            ListaTokens = analizadorER.Parsear(listaTokens);
                            for (int i = 0; i < ListaTokens.Count; i++)
                            {
                                Token token = ListaTokens.ElementAt(i);
                                if (token.GetTipo() == Token.Tipo.INDEFINIDO)
                                {
                                    ListaTokens.Remove(token);
                                    ListaErrores.AddLast(token);
                                }
                            }
                            //Se intenta recuperar las imagenes generadas.
                            if (analizadorER.RutasImagenes.Count > 0)
                            {
                                ListaImagenes = analizadorER.RutasImagenes;
                                pictureBox1.Visible = true;
                                pictureBox1.Image = System.Drawing.Image.FromFile(ListaImagenes.First.Value);
                                imagen_actual = 0;
                                if (ListaImagenes.Count > 1)
                                {
                                    button3.Enabled = true;
                                }
                            }
                        }
                        else
                        {
                            richTextBox1.Text += "Se encontraron errores léxicos. No puede iniciar análsis sintáctico\n";
                        }
                    }
                    else
                    {
                        richTextBox1.Text += "El analisis lexico no devolvió ningún token. No puede iniciar análsis sintáctico\n";
                    }
                    //Se imprimen los resultados del análisis sintáctico.
                    if (!analizadorER.existenciaErrorSintactico)
                    {
                        richTextBox1.Text += analizadorER.consola;
                        richTextBox1.Text += "*SIN ERRORES SINTACTICOS*\n**Finaliza Análisis Sintáctico**\n";
                    }
                    else
                    {
                        richTextBox1.Text += analizadorER.consola + "**Finaliza Análisis Sintáctico**\n";
                    }
                }
            }
        }
        //Imagen Anterior
        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = System.Drawing.Image.FromFile(ListaImagenes.ElementAt(--imagen_actual));
            if (imagen_actual == 0)
            {
                button2.Enabled = false;
                button3.Enabled = true;
            }
        }
        //Imagen Siguiente
        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = System.Drawing.Image.FromFile(ListaImagenes.ElementAt(++imagen_actual));
            button2.Enabled = true;
            if (imagen_actual == ListaImagenes.Count - 1)
            {
                button3.Enabled = false;
            }
        }
        private void guardarTokensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ListaTokens.Count != 0)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Archivos XML | *.xml";
                    saveFileDialog.ShowDialog();
                    String ruta = saveFileDialog.FileName;
                    String contenido = "<?xml version=\"1.0\" encoding=\"UTF - 8\"?>\n" +
                        "<ListaTokens>\n";
                    foreach (Token token in ListaTokens)
                    {
                        contenido += "\t<Token>\n" +
                            "\t\t<Nombre>" + token.GetTipo() + "</Nombre>\n" +
                            "\t\t<Valor>" + token.GetValor() + "</Valor>\n" +
                            "\t\t<Fila>" + token.GetFila() + "</Fila>\n" +
                            "\t\t<Columna>" + token.GetColumna() + "</Columna>\n" +
                            "\t</Token>\n";
                    }
                    contenido += "</ListaTokens>";
                    File.WriteAllText(ruta, contenido);
                }
                else
                {
                    MessageBox.Show("Es necesario analizar una entrada y validar sus lexemas antes de generar este reporte");
                }
            }
            catch (ArgumentException)
            {
            }
        }
        private void guardarErroresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ListaErrores.Count != 0)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Archivos XML | *.xml";
                    saveFileDialog.ShowDialog();
                    String ruta = saveFileDialog.FileName;
                    String contenido = "<?xml version=\"1.0\" encoding=\"UTF - 8\"?>\n" +
                        "<ListaErrores>\n";
                    foreach (Token error in ListaErrores)
                    {
                        contenido += "\t<Error>\n" +
                            "\t\t<Valor>" + error.GetValor() + "</Valor>\n" +
                            "\t\t<Fila>" + error.GetFila() + "</Fila>\n" +
                            "\t\t<Columna>" + error.GetColumna() + "</Columna>\n" +
                            "\t</Error>\n";
                    }
                    contenido += "</ListaErrores>";
                    File.WriteAllText(ruta, contenido);
                }
                else
                {
                    MessageBox.Show("No se encontraron errores durante el análisis");
                }
            }
            catch (ArgumentException)
            {
            }
        }
        private void erroresLéxicosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ListaErrores.Count != 0)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Archivos PDF | *.pdf";
                    saveFileDialog.ShowDialog();
                    String ruta = saveFileDialog.FileName;
                    Document document = new Document(PageSize.LETTER, 10, 10, 10, 10);
                    PdfWriter pw = PdfWriter.GetInstance(document, new FileStream(ruta, FileMode.Create));
                    document.Open();
                    Paragraph encabezado = new Paragraph("Errores léxicos");
                    encabezado.Alignment = Element.ALIGN_CENTER;
                    document.Add(encabezado);
                    PdfPTable tabla = new PdfPTable(4);
                    tabla.AddCell("#");
                    tabla.AddCell("Valor");
                    tabla.AddCell("Fila");
                    tabla.AddCell("Columna");
                    int contadorError = 1;
                    foreach (Token error in ListaErrores)
                    {
                        tabla.AddCell(contadorError.ToString());
                        tabla.AddCell(error.GetValor());
                        tabla.AddCell(error.GetFila().ToString());
                        tabla.AddCell(error.GetColumna().ToString());
                    }
                    document.Add(tabla);
                    document.Close();
                    MessageBox.Show("Se ha creado el archivo PDF");
                }
                else
                {
                    MessageBox.Show("No se encontraron errores durante el análisis");
                }
            }
            catch (ArgumentException)
            {
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
