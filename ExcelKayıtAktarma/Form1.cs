using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

namespace ExcelKayıtAktarma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Sql Bağlantı
        public SqlConnection baglantı = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Ogrenci.mdf;Integrated Security=True");
        //Sql komutu 
        public SqlCommand komut = new SqlCommand();
        //Data adapter veri adaptörü 
        public SqlDataAdapter adaptör = new SqlDataAdapter();
        //Data set veri seti
        public DataSet dt = new DataSet();

        private void Form1_Load(object sender, EventArgs e)
        {
            gridDoldur();
        }
        void gridDoldur()
        {
            //Adatörle bağlantı yapıldı
            adaptör = new SqlDataAdapter("Select * from Ogrenci ", baglantı);
            //Datasete veri aktarıldı
            dt= new DataSet();
            //Bağlantı açıldı
            baglantı.Open();
            //Adaptör ile data seti doldur ogrenci tablosu ile
            adaptör.Fill(dt, "Ogrenci");
            //DataGridviewin kaynadğına data sete verileri aktar.
            dataGridView1.DataSource = dt.Tables["Ogrenci"];
            baglantı.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {  //Kaydetme
            baglantı.Open();
            komut.Connection = baglantı;
            komut.CommandText = "insert into ogrenci(NUMARA,ADSOYAD,BOLUM,KAYITTARIHI) values('"+Text_ogrno.Text+ "','"+Text_adsoyad.Text+ "','"+comboBox1.Text+ "','"+Text_KayıtTarihi.Text+"')";
            komut.ExecuteNonQuery();
            baglantı.Close() ;
            MessageBox.Show("Kayıt eklendi.");
            gridDoldur();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
                //excele kaydet
                //Excel kütüphanesini aktife getir
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = true; // exceli aç 
                object Missing = Type.Missing; //Uyarıları çalıştır
                Workbook workbook = excel.Workbooks.Add(Missing);
                Worksheet sheet = (Worksheet)workbook.Sheets[1];
                int satırRow = 1;
                int sutunCol = 1;
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    Range myRange = (Range)sheet.Cells[sutunCol, satırRow + j];
                    myRange.Value2 = dataGridView1.Columns[j].HeaderText;
                }
                sutunCol++;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        Range myrange = (Range)sheet.Cells[sutunCol + i, satırRow + j];
                        myrange.Value2 = dataGridView1[j, i].Value == null ? " " : dataGridView1[j, i].Value;
                        myrange.Select();
                    }

                }
         
        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //güncelle
            baglantı.Open();
            komut.Connection = baglantı;
            komut.CommandText = "update Ogrenci set ADSOYAD='"+Text_adsoyad.Text+"',BOLUM='"+comboBox1.Text+"',KAYITTARIHI='"+Text_KayıtTarihi.Text+"' where NUMARA ='" + Text_ogrno.Text + "'";
            komut.ExecuteNonQuery();
            baglantı.Close();
            MessageBox.Show("Kayıt güncellendi.");
            gridDoldur();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Sil 
            baglantı.Open();
            komut.Connection = baglantı;
            komut.CommandText = "Delete from ogrenci where NUMARA = '"+Text_ogrno.Text+"'";
            komut.ExecuteNonQuery();
            baglantı.Close();
            MessageBox.Show("Kayıt silindi.");
            gridDoldur();
        }
    }
}
