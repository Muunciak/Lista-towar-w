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

namespace Lista_towarów
{
    public partial class csvReader : Form
    {
        public csvReader()
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
                opf.Filter = "csv File|*.csv| Excel Files(.xls)|*.xls| Excel Files(.xlsx)| *.xlsx | Excel Files(*.xlsm) | *.xlsm";
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
            int selectedRowIndex = dgvDataList.CurrentRow.Index;
            int columnIndex = dgvDataList.CurrentCell.ColumnIndex;

            if (columnIndex == 3)
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
    }
}

