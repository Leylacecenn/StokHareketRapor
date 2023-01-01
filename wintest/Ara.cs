using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wintest
{
    public partial class Ara : UserControl
    {


        SqlConnection cn = new SqlConnection("Server=DESKTOP-VECEN0I\\MSSQLSERVER1; Database=Test; uid=sa; pwd=7521013;");
        public Ara()
        {
            InitializeComponent();
        }
        public string KOD;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "arama";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@arama", textBox1.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                cn.Close();

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //bir seçim yapıldığında ekran kapatılıyor veri form1 in textboxına aktarılıyor
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            arama();

        }

        private void arama()
        {
            DataGridViewRow Satir = dataGridView1.CurrentRow;
            Form1.instance.txtkod.Visible = true;
            Form1.instance.txtkod.Text = Satir.Cells["MalKodu"].Value.ToString();
            this.Visible = false;
        }

        //ilk açıldığında stok kodlarını çekiyor
        private void Ara_Load(object sender, EventArgs e)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "arama";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@arama", textBox1.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            arama();
        }
    }
}
