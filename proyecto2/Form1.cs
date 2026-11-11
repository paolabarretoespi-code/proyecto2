using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Text;

namespace proyecto2
{
    public partial class Form1 : Form
    {
    List<Alumno>  lista = new List<Alumno>();
        string ruta = "alumnos.txt";
        public Form1()
        {
            InitializeComponent();
            Cargar();
            Actualizar();
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        void Actualizar()
        {
            dgvAlumnos.DataSource = null;
            dgvAlumnos.DataSource = lista.ToList();

            int apro = lista.Count(x => x.Promedio >= 6);
            int repro = lista.Count (x => x.Promedio < 6);

            lblAprobados.Text = "Aprobados: " + apro.ToString();
            lblReprobados.Text = "Reprobados: " + repro.ToString();
            Guardar();

            lblAprobados.ForeColor = System.Drawing.Color.Green;
            lblReprobados.ForeColor = System.Drawing.Color.Red;
        }
        void Guardar()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(ruta))
                {
                    foreach (var a in lista)
                    {
                        sw.WriteLine($"{a.Nombre}|{a.Grupo}|{a.Cal1}|{a.Cal2}|{a.Cal3}|{a.Cal4}|{a.Cal5}");
                    }
                }
            }
            catch { }
            }
        void Cargar()
        {
            try
            {
            if(File.Exists(ruta))
                {
                    lista.Clear();
                    foreach (var linea in File.ReadAllLines(ruta))
                    {
                        var p = linea.Split(new char[] {'|'});
                        if (p.Length==7)
                        {
                            lista.Add(new Alumno
                            {
                                Nombre = p[0],
                                Grupo = p[1],
                                Cal1 = double.Parse(p[2]),
                                Cal2 = double.Parse(p[3]),
                                Cal3 = double.Parse(p[4]),
                                Cal4 = double.Parse(p[5]),
                                Cal5 = double.Parse(p[6])
                            });
                            
                        }
                      
                   }
                }
            }
            catch {}
        }
        
        
            
            

            
        
        private void btnAgregar_Click(object sender, EventArgs e)
        {


           
            double cal;
            if (!double.TryParse(txtCal1.Text, out cal) || cal > 10 || cal < 0 ||
                !double.TryParse(txtCal2.Text, out cal) || cal > 10 || cal < 0 ||
                !double.TryParse(txtCal3.Text, out cal) || cal > 10 || cal < 0 ||
                !double.TryParse(txtCal4.Text, out cal) || cal > 10 || cal < 0 ||
                !double.TryParse(txtCal5.Text, out cal) || cal > 10 || cal < 0)
            {
                MessageBox.Show("Pon solo numeros del 0 al 10 en calificaciones");
                return;
            }

            if (txtNombre.Text.Any(char.IsDigit))
            {
                MessageBox.Show("El nombre no puede llevar numeros");
                return;
            }

            if (txtNombre.Text == "" || txtGrupo.Text == "" || txtCal1.Text == "" || txtCal2.Text == "" || txtCal3.Text == "" || txtCal4.Text == "" || txtCal5.Text == "")
            {
                MessageBox.Show("No dejes casillas vacias");
                return;
            }

           
            lista.Add(new Alumno
            {
                Nombre = txtNombre.Text,
                Grupo = txtGrupo.Text,
                Cal1 = double.Parse(txtCal1.Text),
                Cal2 = double.Parse(txtCal2.Text),
                Cal3 = double.Parse(txtCal3.Text),
                Cal4 = double.Parse(txtCal4.Text),
                Cal5 = double.Parse(txtCal5.Text)
            });
            Actualizar();


        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvAlumnos.CurrentRow == null || dgvAlumnos.CurrentRow.DataBoundItem == null)
            {
                MessageBox.Show("Selecciona un alumno de la tabla primero");
                return;
            }

            double cal;
            if (!double.TryParse(txtCal1.Text, out cal) || cal > 10 || cal < 0 ||
                !double.TryParse(txtCal2.Text, out cal) || cal > 10 || cal < 0 ||
                !double.TryParse(txtCal3.Text, out cal) || cal > 10 || cal < 0 ||
                !double.TryParse(txtCal4.Text, out cal) || cal > 10 || cal < 0 ||
                !double.TryParse(txtCal5.Text, out cal) || cal > 10 || cal < 0)
            {
                MessageBox.Show("Pon solo numeros del 0 al 10");
                return;
            }

            var alumnoSeleccionado = dgvAlumnos.CurrentRow.DataBoundItem as Alumno;
            int i = lista.IndexOf(alumnoSeleccionado);

            if (i >= 0)
            {
                lista[i].Nombre = txtNombre.Text;
                lista[i].Grupo = txtGrupo.Text;
                lista[i].Cal1 = double.Parse(txtCal1.Text);
                lista[i].Cal2 = double.Parse(txtCal2.Text);
                lista[i].Cal3 = double.Parse(txtCal3.Text);
                lista[i].Cal4 = double.Parse(txtCal4.Text);
                lista[i].Cal5 = double.Parse(txtCal5.Text);
                Actualizar();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvAlumnos.CurrentRow == null || dgvAlumnos.CurrentRow.DataBoundItem == null) return;
            if (MessageBox.Show("¿Seguro que quieres eliminar?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.No) return;
            var alumnoSeleccionado = dgvAlumnos.CurrentRow.DataBoundItem as Alumno;
            lista.Remove(alumnoSeleccionado);
            Actualizar();
        }

        

        

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var f = lista.Where(x => x.Nombre.Contains(txtBuscar.Text)).ToList();
            dgvAlumnos.DataSource = f;
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel CSV (*.csv)|*.csv";
            sfd.FileName = "alumnos.csv";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                   
                    sw.WriteLine("Nombre,Grupo,Cal1,Cal2,Cal3,Cal4,Cal5,Promedio,Estado");

                    foreach (var a in lista)
                    {
                        sw.WriteLine($"{a.Nombre},{a.Grupo},{a.Cal1},{a.Cal2},{a.Cal3},{a.Cal4},{a.Cal5},{a.Promedio:0.0},{a.Estado}");
                    }
                }
                MessageBox.Show("Exportado a Excel correctamente en:\n" + sfd.FileName);
            }
        }

        private void dgvAlumnos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex < 0) return;
            var a = dgvAlumnos.Rows[e.RowIndex].DataBoundItem as Alumno;
            if (a == null) return;

            txtNombre.Text = a.Nombre;
            txtGrupo.Text = a.Grupo;
            txtCal1.Text = a.Cal1.ToString();
            txtCal2.Text = a.Cal2.ToString();
            txtCal3.Text = a.Cal3.ToString();
            txtCal4.Text = a.Cal4.ToString();
            txtCal5.Text = a.Cal5.ToString();

            lblPromedio.Text = "Promedio:" + a.Promedio.ToString("0.00");
            lblMensaje.Text = "Estado:" + a.Estado;
            lblPromedio.Text = "Promedio:" + a.Promedio.ToString("0.00");
        }
        private void dgvAlumnos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvAlumnos_CellClick(sender, e);
        } 
    }
  }
