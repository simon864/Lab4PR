using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;

namespace sem2_lab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int countRow = 1;
        int countColumn = 1;

        int maxCountValueRow = 0;
        int maxCountValueColumn = 0;
        int countValue = 0;

        double?[,] arr;

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows[0].HeaderCell.Value = "1";

            dataGridView2.AllowUserToAddRows = false;

            radioButton1.Checked = true;
            label1.Text = string.Empty;

            openFileDialog1.Filter = "Excel File|*.xlsx;*.xls";
            openFileDialog1.DefaultExt = ".xls";
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ++countRow;

            dataGridView1.Rows[e.RowIndex].HeaderCell.Value = countRow.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            countValue = 0;
            label1.Text = string.Empty;

            try
            {
                ReadDataGrid();
            }
            catch (Exception ex)
            {
                label1.Text = ex.Message + "\n\n";

                return;
            }

            SortMethods.countValue = countValue;

            if (radioButton1.Checked)
            {
                if (checkBox1.Checked)
                {
                    dataGridView2.Columns.Clear();

                    double?[,] arrBubble = arr.Clone() as double?[,];

                    SortMethods.BubbleSort(arrBubble);

                    WriteToGrid(arrBubble, dataGridView2);

                    label1.Text = $"Время выполнения библа: {SortMethods.timeExecution[0]}\n" +
                      $"Количество итераций библа: {SortMethods.iters[0]}\n";
                }

                if (checkBox2.Checked)
                {
                    dataGridView2.Columns.Clear();

                    double?[,] arrPast = arr.Clone() as double?[,];

                    SortMethods.PastSort(arrPast);

                    WriteToGrid(arrPast, dataGridView2);

                    label1.Text += $"Время выполнения сортировки вставками: {SortMethods.timeExecution[1]}\n" +
                      $"Количество итераций сортировки вставками: {SortMethods.iters[1]}\n";
                }

                if (checkBox3.Checked)
                {
                    dataGridView2.Columns.Clear();

                    double?[,] arrShaker = arr.Clone() as double?[,];

                    SortMethods.ShakerSort(arrShaker);

                    WriteToGrid(arrShaker, dataGridView2);

                    label1.Text += $"Время выполнения шейкерной сортировки: {SortMethods.timeExecution[2]}\n" +
                      $"Количество итераций шейкерной сортировки: {SortMethods.iters[2]}\n";
                }

                if (checkBox4.Checked)
                {
                    dataGridView2.Columns.Clear();

                    double?[,] arrQuick = arr.Clone() as double?[,];

                    SortMethods.QuickSort(arrQuick);

                    WriteToGrid(arrQuick, dataGridView2);

                    label1.Text += $"Время выполнения быстрой сортировки: {SortMethods.timeExecution[3]}\n" +
                      $"Количество итераций быстрой сортировки: {SortMethods.iters[3]}\n";
                }

                if (checkBox5.Checked)
                {
                    dataGridView2.Columns.Clear();

                    double?[,] arrBogo = arr.Clone() as double?[,];

                    SortMethods.BogoSort(arrBogo);

                    WriteToGrid(arrBogo, dataGridView2);

                    label1.Text += $"Время выполнения случайной (bogo) сортировки: {SortMethods.timeExecution[4]}\n" +
                      $"Количество итераций bogo: {SortMethods.iters[4]}\n";
                }
            }
            else
            {
                if (checkBox1.Checked)
                {
                    dataGridView2.Columns.Clear();

                    double?[,] arrBubble = SortMethods.BubbleSortRev(arr);

                    WriteToGrid(arrBubble, dataGridView2);

                    label1.Text = $"Время выполнения пузырьковой сортировки: {SortMethods.timeExecution[0]}\n" +
                      $"Количество итераций пузырька: {SortMethods.iters[0]}\n";
                }

                if (checkBox2.Checked)
                {
                    dataGridView2.Columns.Clear();

                    double?[,] arrPast = SortMethods.PastSortRev(arr);

                    WriteToGrid(arrPast, dataGridView2);

                    label1.Text += $"Время выполнения сортировки вставками: {SortMethods.timeExecution[1]}\n" +
                      $"Количество итераций bogo: {SortMethods.iters[1]}\n";
                }

                if (checkBox3.Checked)
                {
                    dataGridView2.Columns.Clear();

                    double?[,] arrShaker = SortMethods.ShakerSortRev(arr);

                    WriteToGrid(arrShaker, dataGridView2);

                    label1.Text += $"Время выполнения шейкерной сортировки: {SortMethods.timeExecution[2]}\n" +
                      $"Количество итераций шейкерной сортировки: {SortMethods.iters[2]}\n";
                }

                if (checkBox4.Checked)
                {
                    dataGridView2.Columns.Clear();

                    double?[,] arrQuick = SortMethods.QuickSortRev(arr);

                    WriteToGrid(arrQuick, dataGridView2);

                    label1.Text += $"Время выполнения быстрой сортировки: {SortMethods.timeExecution[3]}\n" +
                      $"Количество итераций быстрой сортировки: {SortMethods.iters[3]}\n";
                }

                if (checkBox5.Checked)
                {
                    dataGridView2.Columns.Clear();

                    double?[,] arrBogo = SortMethods.BogoSortRev(arr);

                    WriteToGrid(arrBogo, dataGridView2);

                    label1.Text += $"Время выполнения случайной (bogo) сортировки: {SortMethods.timeExecution[4]}\n" +
                      $"Количество итераций bogo: {SortMethods.iters[4]}\n";
                }
            }
        }

        private void ReadDataGrid()
        {
            countRow = dataGridView1.RowCount;
            countColumn = dataGridView1.ColumnCount;
            maxCountValueColumn = 0;
            maxCountValueRow = 0;

            RealSizeDataGrid(); // отбрасывание null-значащих строк и столбцов => новый размер для таблицы

            CountValueDataGrid(); // максимальное кол-во ненулевых значений в строках и столбцах

            if (countValue == 0)
            {
                throw new Exception("В таблице нет значений !!!");
            }

            if (countValue == 1)
            {
                throw new Exception("Одно значение не нужно сортировать !!!");
            }

            if (maxCountValueColumn * maxCountValueRow < countValue)
            {
                maxCountValueRow = (int)Math.Ceiling(Math.Sqrt(countValue));
                maxCountValueColumn = maxCountValueRow - 1;

                //if (countValue % 2 == 0) {
                //  maxCountValueColumn = countValue / 2;
                //  maxCountValueRow = maxCountValueColumn;
                //} else {
                //  maxCountValueColumn = (countValue + 1) / 2;
                //  maxCountValueRow = maxCountValueColumn - 1;
                //}
            }

            arr = new double?[maxCountValueColumn, maxCountValueRow];

            int indexRow = 0;
            int indexColumn = 0;

            for (int indexRowDataGrid = 0; indexRowDataGrid < dataGridView1.RowCount; ++indexRowDataGrid)
            {
                for (int indexColumnDataGrid = 0; indexColumnDataGrid < dataGridView1.ColumnCount; ++indexColumnDataGrid)
                {
                    if (dataGridView1[indexColumnDataGrid, indexRowDataGrid].Value == null ||
                      dataGridView1[indexColumnDataGrid, indexRowDataGrid].Value.ToString() == string.Empty)
                    {
                        continue;
                    }

                    arr[indexRow, indexColumn] = Convert.ToDouble(dataGridView1[indexColumnDataGrid, indexRowDataGrid].Value.ToString());

                    ++indexColumn;

                    if (indexColumn >= maxCountValueRow)
                    {
                        ++indexRow;

                        if (indexRow >= maxCountValueColumn)
                        {
                            break;
                        }

                        indexColumn = 0;
                    }
                }

                if (indexRow >= maxCountValueColumn)
                {
                    break;
                }
            }
        }

        private void RealSizeDataGrid()
        {
            bool resize = true;

            for (int indexRow = 0; indexRow < dataGridView1.RowCount; ++indexRow)
            {
                resize = true;

                for (int indexColumn = 0; indexColumn < dataGridView1.ColumnCount; ++indexColumn)
                {
                    if (dataGridView1[indexColumn, indexRow].Value != null)
                    {
                        resize = false;

                        break;
                    }
                }

                if (resize)
                {
                    --countRow;
                }
            }

            resize = true;

            for (int indexColumn = 0; indexColumn < dataGridView1.ColumnCount; ++indexColumn)
            {
                resize = true;

                for (int indexRow = 0; indexRow < dataGridView1.RowCount; ++indexRow)
                {
                    if (dataGridView1[indexColumn, indexRow].Value != null)
                    {
                        resize = false;
                        break;
                    }
                }

                if (resize)
                {
                    --countColumn;
                }
            }
        }

        private void CountValueDataGrid()
        {
            int maxCountValue = 0;

            for (int indexRow = 0; indexRow < dataGridView1.RowCount; ++indexRow)
            {
                maxCountValue = 0;

                for (int indexColumn = 0; indexColumn < dataGridView1.ColumnCount; ++indexColumn)
                {
                    if (dataGridView1[indexColumn, indexRow].Value != null)
                    {
                        ++maxCountValue;
                    }
                }

                if (maxCountValue > maxCountValueRow)
                {
                    maxCountValueRow = maxCountValue;
                }
            }

            maxCountValue = 0;

            for (int indexColumn = 0; indexColumn < dataGridView1.ColumnCount; ++indexColumn)
            {
                maxCountValue = 0;

                for (int indexRow = 0; indexRow < dataGridView1.RowCount; ++indexRow)
                {
                    if (dataGridView1[indexColumn, indexRow].Value != null)
                    {
                        ++maxCountValue;
                    }
                }

                if (maxCountValue > maxCountValueColumn)
                {
                    maxCountValueColumn = maxCountValue;
                }
            }

            maxCountValue = 0;

            for (int indexRow = 0; indexRow < dataGridView1.RowCount; ++indexRow)
            {
                for (int indexColumn = 0; indexColumn < dataGridView1.ColumnCount; ++indexColumn)
                {
                    if (dataGridView1[indexColumn, indexRow].Value != null && dataGridView1[indexColumn, indexRow].Value.ToString() != string.Empty)
                    {
                        ++countValue;
                    }
                }
            }
        }

        private void WriteToGrid(double?[,] array, DataGridView dataGrid)
        {
            dataGrid.Columns.Clear();

            for (int indexColumn = 0; indexColumn < maxCountValueRow; ++indexColumn)
            {
                dataGrid.Columns.Add((indexColumn + 1).ToString(), (indexColumn + 1).ToString());
            }

            for (int indexRow = 0; indexRow < maxCountValueColumn; ++indexRow)
            {
                dataGrid.Rows.Add();

                dataGrid.Rows[indexRow].HeaderCell.Value = (indexRow + 1).ToString();
            }

            for (int indexRow = 0; indexRow < array.GetLength(0); ++indexRow)
            {
                for (int indexColumn = 0; indexColumn < array.GetLength(1); ++indexColumn)
                {
                    dataGrid[indexColumn, indexRow].Value = array[indexRow, indexColumn].ToString();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ++countColumn;

            dataGridView1.Columns.Add(countColumn.ToString(), countColumn.ToString());

            if (countRow == 0)
            {
                ++countRow;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (countColumn == 1)
            {
                return;
            }

            try
            {
                dataGridView1.Columns.RemoveAt(countColumn - 1);
            }
            catch
            {
                return;
            }

            --countColumn;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView2.Columns.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;

                if (!Regex.IsMatch(fileName, @"\S+((\.xlsx)|(\.xls))"))
                {
                    DialogResult messageBox = MessageBox.Show(
                      "Неправильный формат файла !!!",
                      "Ошибка",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error,
                      MessageBoxDefaultButton.Button1,
                      MessageBoxOptions.DefaultDesktopOnly);
                }
                else
                {
                    fileNameExcel.Text = fileName;
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void delFileExcel_Click(object sender, EventArgs e)
        {
            fileNameExcel.Text = string.Empty;
        }

        private void readFileExcel_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            Workbook workbook;

            try
            {
                workbook = excel.Workbooks.Open(fileNameExcel.Text);
            }
            catch
            {
                label1.Text = "Неверный путь к Excel файлу !!!";

                return;
            }

            Worksheet worksheet = workbook.Worksheets[1];

            Range cells = worksheet.Range[rangeExcel.Text];

            maxCountValueColumn = cells.Rows.Count;
            maxCountValueRow = cells.Columns.Count;

            arr = new double?[cells.Rows.Count, cells.Columns.Count];



            for (int indexRow = 1; indexRow < cells.Rows.Count + 1; ++indexRow)
            {
                for (int indexColumn = 1; indexColumn < cells.Columns.Count + 1; ++indexColumn)
                {
                    try
                    {
                        arr[indexRow - 1, indexColumn - 1] = cells[indexRow, indexColumn].Value;
                    }
                    catch
                    {
                        label1.Text = $"Неверный формат данных в ячейке [{indexRow}, {indexColumn}]";
                    }
                }
            }

            workbook.Close();

            WriteToGrid(arr, dataGridView1);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(randArrCountRows.Text, out int countRows))
            {
                label1.Text = "Неверное значение для количества строк !!!";

                return;
            }

            if (!int.TryParse(randArrCountColumns.Text, out int countColumns))
            {
                label1.Text = "Неверное значение для количества столбцов !!!";

                return;
            }

            if (!int.TryParse(randArrMinVal.Text, out int minVal))
            {
                label1.Text = "Неверное значение для минимального значения в массиве !!!";

                return;
            }

            if (!int.TryParse(randArrMaxVal.Text, out int maxVal))
            {
                label1.Text = "Неверное значение для максимального значения в массиве !!!";

                return;
            }

            if (countColumns > 655)
            {
                label1.Text = "Количество столбцов не должно превышать 655 !!!";

                return;
            }

            maxCountValueColumn = countRows;
            maxCountValueRow = countColumns;

            double?[,] arrRand = RandomArray.CreateRandonArray(countRows, countColumns, minVal, maxVal);

            WriteToGrid(arrRand, dataGridView1);
        }
    }
}