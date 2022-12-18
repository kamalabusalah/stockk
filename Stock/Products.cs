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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Stock
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            cmbStatus.SelectedIndex = 0;
            LoadData();

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            // insert
            con.Open();
            bool status = false;
            if (cmbStatus.SelectedIndex == 0)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            var sqlQuery = "";

            if (IfProductExists(con, txtProductCode.Text))
            {
                sqlQuery = @"UPDATE [Stock].[dbo].[Products] SET [ProductName] = '" + txtProductName.Text + "' ," +
                    " [ProductStatus] = '" + status + "'  WHERE [ProductCode]= '" + txtProductCode.Text + "'";
            }
            else
            {
                 sqlQuery = @"INSERT INTO [Stock].[dbo].[Products] ([ProductCode],[ProductName], [ProductStatus]) " +
                "  VALUES('" + txtProductCode.Text + "','" + txtProductName.Text + "' ,'" + status + "')";
            }
            
            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            cmd.ExecuteNonQuery();


            con.Close();

            // reading data
            LoadData();

        }

        private bool IfProductExists(SqlConnection con ,string productCode)
        {

            SqlDataAdapter sda = new SqlDataAdapter("Select * from [Stock].[dbo].[Products]" +
                " where [ProductCode]='" + txtProductCode.Text + "'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)

                return true;
            else
                return false;

        }

        public void LoadData()
        {

            SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            SqlDataAdapter sda = new SqlDataAdapter("Select * from [Stock].[dbo].[Products]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            dataGridView.Rows.Clear();

            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView.Rows.Add();
                dataGridView.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                dataGridView.Rows[n].Cells[1].Value = item["ProductName"].ToString();
                if ((bool)item["ProductStatus"])
                {
                    dataGridView.Rows[n].Cells[2].Value = "Active";
                }
                else
                {
                    dataGridView.Rows[n].Cells[2].Value = "Deactive";
                }

            }
        }

        private void dataGridView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtProductCode.Text = dataGridView.SelectedRows[0].Cells[0].Value.ToString();
            txtProductName.Text = dataGridView.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridView.SelectedRows[0].Cells[2].Value.ToString()== "Active")
            {
                cmbStatus.SelectedIndex = 0;
            }
            else
            {
                cmbStatus.SelectedIndex = 1;
            }
            
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            var sqlQuery = "";

            if (IfProductExists(con, txtProductCode.Text))
            {
                con.Open();

                sqlQuery = @"Delete from  [Stock].[dbo].[Products]  WHERE [ProductCode] = '" + txtProductCode.Text + "'";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();

                con.Close();
            }
            else
            {
                MessageBox.Show("Record Not Exists...!");
            }

            con.Close();

            
            LoadData();

        }
    }
}
