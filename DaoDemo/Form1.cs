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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DbDataContext db = new DbDataContext();
            var data = db.ABS.Where(p => p.BDATE >= new DateTime(2022, 1, 1) && p.BDATE <= new DateTime(2022, 1, 31));
            data = data.Where(p => p.NOBR == "H000003");
            var hours = data.First().TOL_HOURS;
            MessageBox.Show(hours.ToString());
            dataGridView1.DataSource = data;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DbDataContext db = new DbDataContext();
            var data = from a in db.ABS
                       join b in db.BASE on a.NOBR equals b.NOBR
                       where a.BDATE >= new DateTime(2022, 1, 1) && a.BDATE <= new DateTime(2022, 1, 31)
                       select new TestDto { 員工編號 = a.NOBR, 員工姓名 = b.NAME_C, 請假日期 = a.BDATE, 開始時間 = a.BTIME, 結束時間 = a.ETIME };
            var value = 1234;
            dataGridView1.DataSource = data;
        }
        public List<TestDto> getData()
        {
            DbDataContext db = new DbDataContext();
            var data = from a in db.ABS
                       join b in db.BASE on a.NOBR equals b.NOBR
                       where a.BDATE >= new DateTime(2022, 1, 1) && a.BDATE <= new DateTime(2022, 1, 31)
                       select new TestDto { 員工編號 = a.NOBR, 員工姓名 = b.NAME_C, 請假日期 = a.BDATE, 開始時間 = a.BTIME, 結束時間 = a.ETIME };
            return data.ToList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //MessageBox.Show(Properties.Settings.Default.EMCCNConnectionString);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var sqlconnection = new SqlConnection(Properties.Settings.Default.EMCCNConnectionString);
            using (sqlconnection)
            {
                if (sqlconnection.State != ConnectionState.Open)
                    sqlconnection.Open();

                //SqlCommand sqlCommand = new SqlCommand(@"select * from ABS 
                //                    where BDATE>='20220101' and BDATE<='20220131'
                //                    and NOBR='H000003'", sqlconnection);

                SqlCommand sqlCommand = new SqlCommand(textBox1.Text, sqlconnection);

                var dr = sqlCommand.ExecuteReader();
                var dataTable = new DataTable("ABS");
                dataTable.Load(dr);
                dataGridView1.DataSource = dataTable;
            }
        }
    }
    public class TestDto
    {
        public string 員工編號 { get; set; }
        public string 員工姓名 { get; set; }
        public DateTime 請假日期 { get; set; }
        public string 開始時間 { get; set; }
        public string 結束時間 { get; set; }
    }
}
