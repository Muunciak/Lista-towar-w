using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Lista_towarów
{
    public partial class ExcelReader : Form
    {
        public ExcelReader()
        {
            InitializeComponent();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "Excel Files(.csv)|*.csv| Excel Files(.xls)|*.xls| Excel Files(.xlsx)| *.xlsx | Excel Files(*.xlsm) | *.xlsm";
                if (opf.ShowDialog() == DialogResult.OK)
                {
                    string[] Lines = File.ReadAllLines(opf.FileName);
                    string[] Fields;
                    Fields = Lines[0].Split(new char[] { ';' });
                    int Cols = Fields.GetLength(0);
                    DataTable dt = new DataTable();
                    for (int i = 0; i < Cols; i++)
                        dt.Columns.Add(Fields[i].ToLower(), typeof(string));
                    DataRow Row;
                    for (int i = 1; i < Lines.GetLength(0); i++)
                    {
                        Fields = Lines[i].Split(new char[] { ';' });
                        Row = dt.NewRow();
                        for (int f = 0; f < Cols; f++)
                            Row[f] = Fields[f];
                        dt.Rows.Add(Row);
                    }
                    dgvDataList.DataSource = dt;
                }
            }
        }
        private void dgvDataList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnEditCode_Click(object sender, EventArgs e)
        {
            int columnIndex = dgvDataList.CurrentCell.ColumnIndex;
            int checker = dgvDataList.Columns["KOD"].Index; ;

            if (columnIndex == checker)
            {
                dgvDataList.BeginEdit(true);
            }
            else
            {
                MessageBox.Show("Mozesz edytowac tylko kod!");
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void btnInsertDB_Click(object sender, EventArgs e)
        {
             int columnIndex = dgvDataList.CurrentCell.ColumnIndex, s,i;
            string columnName = dgvDataList.Columns[columnIndex].Name;
            Regex regex = new Regex(@"Column[0-9]*");
            string cs = @"Data Source=DESKTOP-JT8L13H\SQLEXPRESS;Initial Catalog=DB1;Integrated Security=True";
            string StrQuery;
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        conn.Open();
                        s = 0;
                        for (columnIndex = 0; columnIndex < dgvDataList.Columns.Count; columnIndex++)
                        {
                            if (s == 0)
                            {
                                StrQuery = @"CREATE TABLE FileTable (ID INT NOT NULL);";
                                comm.CommandText = StrQuery;
                                comm.ExecuteNonQuery();
                                s++;
                            }
                            columnName = dgvDataList.Columns[columnIndex].Name;
                            if (!regex.IsMatch(columnName))
                            { 
                                    StrQuery = @"ALTER TABLE FileTable ADD [" + columnName + "] text;";
                                    comm.CommandText = StrQuery;
                                    comm.ExecuteNonQuery();
                            }
                        }
                        for(int k=0; k<dgvDataList.Rows.Count - 1; k++)
                        { 
                            StrQuery = @"INSERT INTO FileTable (ID) VALUES (" + k + ");";
                            comm.CommandText = StrQuery;
                            comm.ExecuteNonQuery();
                            MessageBox.Show(StrQuery);
                        }
                        for (columnIndex = 0; columnIndex < dgvDataList.Columns.Count - 1; columnIndex++)
                        { 
                            for (i = 0; i < dgvDataList.Rows.Count - 1; i++)
                            {
                                columnName = dgvDataList.Columns[columnIndex].Name;
                                if (!regex.IsMatch(columnName))
                                {
                                        StrQuery = @"UPDATE FileTable SET [" + columnName + "] = '(" + dgvDataList.Rows[i].Cells[columnName].Value + ")' WHERE ID = '" + i + "';";
                                        comm.CommandText = StrQuery;
                                        comm.ExecuteNonQuery();
                                        MessageBox.Show(StrQuery);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex.Message);
            }
            finally
            {
                MessageBox.Show("Baza danych została zaktualizowana.");
            }
        }
    }
}
