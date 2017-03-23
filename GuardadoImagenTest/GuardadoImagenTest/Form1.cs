using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing.Imaging;
using System.Data;

namespace GuardadoImagenTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        MySqlConnection Cnn = new MySqlConnection("server=localhost;database=Human;Uid=root;pwd=hola");

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            Cnn.Open();
            if (txtnombre.Text == "")
            {
                MessageBox.Show("Ningun campo debe estar vacio", "Error");
            }
            else
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("Insert into test(nombre,imagen)values('"+txtnombre.Text+"',@imagen) ",Cnn);
                    MemoryStream ms = new MemoryStream();

                    pBlimagen.Image.Save(ms, ImageFormat.Jpeg);
                    byte[] aByte = ms.ToArray();
                    cmd.Parameters.AddWithValue("imagen", aByte);
                    int i = 0;
                    i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        MessageBox.Show("Datos guardados correctamente", "Guardar");
                    }
                }
                catch (Exception)
                {

                }
            }
            Cnn.Close();
        }

        private void verToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySqlCommand comando = new MySqlCommand("Select Imagen from test where Id=1",Cnn);
            MySqlDataAdapter dp = new MySqlDataAdapter(comando);
            DataSet ds = new DataSet("Imagen");
            dp.Fill(ds, "Imagen");
            byte[] DAtos = new byte[0];
            DataRow DR;
            DR = ds.Tables["Imagen"].Rows[0];

            DAtos = (byte[])DR["Imagen"];

            System.IO.MemoryStream ms = new System.IO.MemoryStream(DAtos);
            pictureBox1.Image = System.Drawing.Bitmap.FromStream(ms);
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            ofd.Filter = "Archivo de Imagen | *.jpg| Archivo PNG|*.png| Todos los archivos|*.*";
            DialogResult resultado = ofd.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                pBlimagen.Image = Image.FromFile(ofd.FileName);
            }
        }  

    }
}
