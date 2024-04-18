using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class AuthForm : Form
    {
        public AuthForm()
        {
            InitializeComponent();
            itemBindingNavigator.Visible = false;
            itemDataGridView.Visible = false;
            itemBindingNavigator.MoveNextItem.Visible = false;
            itemBindingNavigator.CountItem.Visible = false;
            itemBindingNavigator.PositionItem.Visible = false;
            itemBindingNavigator.MoveFirstItem.Visible = false;
            itemBindingNavigator.MoveLastItem.Visible = false;
            itemBindingNavigator.MovePreviousItem.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(Session.dbConnectionString))
            {
                conn.Open();
                string SqlSelectAllItem = $"SELECT * FROM Users WHERE Username = '{textBox1.Text}' AND Password = '{textBox2.Text}'";
                SqlCommand cmd = new SqlCommand(SqlSelectAllItem, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        label1.Text = $"Вы авторизованы как: {Session.CurrentUsername = reader.GetString(1)}";
                        label1.Visible = true;
                    }
                    button1.Visible = false;
                    button2.Visible = true;
                    button3.Visible = true;
                    textBox1.Visible = false;
                    textBox2.Visible = false;
                }
                else
                {
                    MessageBox.Show("Нет пользователя");
                }
                conn.Close();
            }
            if (Session.CurrentUsername == "admin")
            {
                button4.Visible = true;
            }
        }
        private int offset = 0;
        private const int pageSize = 15;
        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(Session.dbConnectionString))
            {
                conn.Open();
                string sqlSelect = $"SELECT * FROM Item ORDER BY ItemId OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                conn.Close();

                itemDataGridView.DataSource = dt;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            itemBindingNavigator.Visible = true;
            itemDataGridView.Visible = true;
            textBox3.Visible = true;
            button5.Visible = true;
            buttonNext.Visible = true;
            button6.Visible = true;
            comboBox1.Visible = true;
            if (Session.CurrentUsername == "guest")
            {
                itemBindingNavigator.AddNewItem.Visible = false;
                itemBindingNavigator.DeleteItem.Visible = false;
                itemDataGridView.Enabled = false;
            }
            LoadData();
        }

        private void itemBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.itemBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.shopDBDataSet);

        }

        private void AuthForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "shopDBDataSet.Item". При необходимости она может быть перемещена или удалена.
            this.itemTableAdapter.Fill(this.shopDBDataSet.Item);

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(Session.dbConnectionString))
            {
                conn.Open();
                string SqlSelectVubItem = $"SELECT * FROM Item WHERE Name = @Name";
                SqlCommand cmd = new SqlCommand(SqlSelectVubItem, conn);
                cmd.Parameters.AddWithValue("@Name", textBox3.Text);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

               itemDataGridView.DataSource = dt;
                conn.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox3.Text = null;
            using (SqlConnection conn = new SqlConnection(Session.dbConnectionString))
            {
                conn.Open();
                string SqlSelectVubItem = $"SELECT * FROM Item";
                SqlCommand cmd = new SqlCommand(SqlSelectVubItem, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                itemDataGridView.DataSource = dt;
                conn.Close();
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            offset += pageSize;
            LoadData();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (offset != 0)
            {
                offset -= pageSize;
                LoadData();
            }
            else
            {
                MessageBox.Show("ДА");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedOption = comboBox1.SelectedItem.ToString();
            switch (selectedOption)
            {
                case "Все типы":
                    RefreshData();
                    break;
                case "Name Убывание":
                    SortDataByNameDescending();
                    break;
                case "Name Возрастание":
                    SortDataByNameAscending();
                    break;
                default:
                    break;
            }
        }
        private void RefreshData()
        {
            using (SqlConnection conn = new SqlConnection(Session.dbConnectionString))
            {
                conn.Open();
                string sqlselect = "SELECT * FROM Item";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlselect, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                itemDataGridView.DataSource = dt;
                conn.Close();
            }
        }

        private void SortDataByNameDescending()
        {
            using (SqlConnection conn = new SqlConnection(Session.dbConnectionString))
            {
                conn.Open();
                string sqlselect = "SELECT * FROM Item ORDER BY Name DESC";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlselect, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                itemDataGridView.DataSource = dt;
                conn.Close();
            }
        }

        private void SortDataByNameAscending()
        {
            using (SqlConnection conn = new SqlConnection(Session.dbConnectionString))
            {
                conn.Open();
                string sqlselect = "SELECT * FROM Item ORDER BY Name ASC";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlselect, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                itemDataGridView.DataSource = dt;
                conn.Close();
            }
        }
    }
}
