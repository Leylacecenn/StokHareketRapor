using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace wintest
{
    public partial class Form1 : Form
    {
        //veritabanı bağlantısı gerçekleştirdik 
        SqlConnection cn = new SqlConnection("Server=DESKTOP-VECEN0I\\MSSQLSERVER1; Database=Test; uid=sa; pwd=7521013;");
       //formlar arası veri alışverişi için gerekli adımlar
        public static Form1 instance;
        decimal stok = 0;
        public Form1()
        {
            InitializeComponent();
            instance = this;
        }
        //arama kısmı için devexpresde araştırmalarıma göre kullanılabilecek
        //tool'lar mevcut ama elimdeki imkanlar ile ona benzer bir yapı oluşturdum 
        //burada textbox içinde herhangi bir yazı eylemi gerçekleşirse arama için usercontrol açılıyor

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Ara ara = new Ara();
            this.Controls.Add(ara);
            ara.Visible = true;
            txtkod.Visible = false;
            ara.BringToFront();
            ara.Location = new Point(29, 37);
        }

        //kullanıcıdan tarih ve malkodu alınıyor
        private void button1_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            int baslangic = Convert.ToInt32((dateTimebaslangic.Value).ToOADate());
            int bitis = Convert.ToInt32((dateTimebitis.Value).ToOADate());
            listeme(txtkod.Text, baslangic, bitis);
        }
        //gelen veriler doğrultusunda listeleme methodu çalıştırılıyor
        public void listeme(string malkodu, int baslangic, int bitis)
        {
            try
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "listeleme";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@arama", malkodu);
                cmd.Parameters.AddWithValue("@baslangictarihi", baslangic);
                cmd.Parameters.AddWithValue("@bitistarihi", bitis);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();                  
                da.Fill(dt);
                //alınan veri klonlanıyor
                DataTable reversedDt = dt.Clone();
                //stok sayımı için tesine çevriliyor
                for (var row = dt.Rows.Count - 1; row >= 0; row--)
                    reversedDt.ImportRow(dt.Rows[row]);
                //eskisine aktarılıyor
                dt = reversedDt;
                //listeleme userkontrol içinde manuel bir liste hazırlanıyor
                foreach (DataRow dr in dt.Rows)
                {
                    liste liste = new liste();
                    liste.lblID.Text = dr["ID"].ToString();
                    liste.lblmalkodu.Text = dr["MalKodu"].ToString();
                    liste.lblIslemturu.Text = dr["IslemTur"].ToString();
                    liste.lblevrakno.Text = dr["EvrakNo"].ToString();
                    liste.lbltarih.Text = dr["Tarih1"].ToString();
                    liste.lblislem.Text = dr["IslemTuru"].ToString();
                    liste.lblmiktar.Text = dr["Miktar"].ToString();
                    liste.lblgiris.Text = dr["Giris"].ToString();
                    liste.lblcikis.Text = dr["Cikis"].ToString();
                    var giris = dr["Giris"].ToString();
                    var cikis = dr["Cikis"].ToString();
                    string islemtur = dr["IslemTur"].ToString();
                    if (islemtur == "0")
                    {
                        stok += Convert.ToDecimal(giris);
                    }
                    else
                    {
                        stok -= Convert.ToDecimal(cikis);
                    }

                    liste.lblstok.Text = stok.ToString();
                    flowLayoutPanel1.Controls.Add(liste);
                    liste.BringToFront();
                }

                cn.Close();

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
    }
}
