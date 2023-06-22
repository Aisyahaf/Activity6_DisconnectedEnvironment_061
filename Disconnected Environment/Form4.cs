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

namespace Disconnected_Environment
{
    public partial class Form4 : Form
    {
        private string stringConnection = "data source = DESKTOP-VA6CQHR\\FITRIAI;" + "database=Praktikum_PABD; User=sa; Password=123";
        private SqlConnection koneksi;

        public void refreshform()
        {
            cbxNama.Enabled = false;
            cbxSM.Enabled = false;
            cbxTahunM.Enabled = false;
            cbxNama.SelectedIndex = -1;
            cbxSM.SelectedIndex = -1;
            cbxTahunM.SelectedIndex = -1;
            txtNIM.Visible = false;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            btnAdd.Enabled = true;
        }

        public Form4()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringConnection);
            refreshform();
        }

        private void dataGridView()
        {
            koneksi.Open();
            string str = "select * from dbo.Status_mahasiswa";
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            koneksi.Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void cbNama()
        {
            

            koneksi.Open();
            string str = "select nama_mahasiswa from dbo.Mahasiswa where not EXISTS (select id_status from dbo.Status_mahasiswa where dbo.Status_mahasiswa.nim = dbo.Status_mahasiswa.nim)";

            SqlCommand cmd = new SqlCommand(str, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteReader();
            koneksi.Close();

            cbxNama.DisplayMember = "nama_mahasiswa";
            cbxNama.ValueMember = "NIM";
            cbxNama.DataSource = ds.Tables[0];
        }

        private void cbTahunMasuk()
        {
            int y = DateTime.Now.Year - 2010;
            string[] type = new string[y];
            int i = 0;
            for (i = 0; i<type.Length; i++)
            {
                if (i == 0)
                {
                    cbxTahunM.Items.Add("2010");
                }
                else
                {
                    int l = 2010 + i;
                    cbxTahunM.Items.Add(l.ToString());
                }
            }
        }

        private void cbxNama_SelectedIndexChanged(object sender, EventArgs e)
        {
            koneksi.Open();
            string nim = "";
            string strs = "select nim from dbo.Mahasiswa where nama_mahasiswa = @nm";
            SqlCommand cm = new SqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@nm", cbxNama.Text));
            SqlDataReader dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    nim = dr["NIM"].ToString();
                }
            dr.Close();
            koneksi.Close();

            txtNIM.Text = nim;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            dataGridView();
            btnOpen.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cbxTahunM.Enabled = true;
            cbxNama.Enabled = true;
            cbxSM.Enabled = true;
            txtNIM.Visible = true;
            btnClear.Enabled = true;
            btnSave.Enabled = true;
            btnAdd.Enabled = true;
            cbTahunMasuk();
            cbNama();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string nim = txtNIM.Text;
            string SM = cbxSM.Text;
            string TahunMsk = cbxTahunM.Text;
            int count = 0;
            string tempKodeStatus = "";
            string kodeStatus = "";
            koneksi.Open();

            string str = "select count(*) from dbo.Status_mahasiswa ";
            SqlCommand cm = new SqlCommand(str, koneksi);
            count = (int)cm.ExecuteScalar();

            if(count == 0)
            {
                kodeStatus = "1";
            }
            else
            {
                string strs = "select Max(id_status) from dbo.Status_mahasiswa";
                SqlCommand cmStatusMahasiswaSum = new SqlCommand(strs, koneksi);
                int totalStatusMahasiswa = (int)cmStatusMahasiswaSum.ExecuteScalar();
                int finalKodeStatusInt = totalStatusMahasiswa + 1;
            }
            string queryString = "insert into dbo.Status_mahasiswa (id_status, nim, status_mahasiswa, tahun_masuk)" + "values(@ids, @NIM, @sm, @tm)";
            SqlCommand cmd = new SqlCommand(queryString, koneksi);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("ids", kodeStatus));
            cmd.Parameters.Add(new SqlParameter("NIM", nim));
            cmd.Parameters.Add(new SqlParameter("sm", SM));
            cmd.Parameters.Add(new SqlParameter("tm", TahunMsk));
            cmd.ExecuteNonQuery();
            koneksi.Close();

            MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            refreshform();
            dataGridView();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void Form4_FormClosed(object sender, EventArgs e)
        {
            Form1 hu = new Form1();
            hu.Show();
            this.Hide();
        }
    }
}
