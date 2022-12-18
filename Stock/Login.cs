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

namespace Stock
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtPassword.Text = "";
            txtUserName.Clear();
            txtUserName.Focus();    

        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            SqlDataAdapter sda = new SqlDataAdapter(@"SELECT *
             FROM [Stock].[dbo].[Login] where UserName='"+txtUserName.Text+"'and Password='"+txtPassword.Text+"'",con);


            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count == 1)
            {

                this.Hide();
                StockMain main = new StockMain();
                main.Show();
            }

            else
            {
                MessageBox.Show("Invalid UserName & Password...!", "Error ",MessageBoxButtons.OK, MessageBoxIcon.Error);
                BtnClear_Click(sender, e);  
            }
        }
    }
}
