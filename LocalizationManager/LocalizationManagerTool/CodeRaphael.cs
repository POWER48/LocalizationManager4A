using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LocalizationManagerTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataTable dataTable = new DataTable { TableName = "TraductionDataTable" };
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string FileName = openFileDialog.FileName;
                string extension = FileName.Split('.')[1];
                using (System.IO.StreamReader streamReader = new StreamReader(FileName))
                {
                    ClearTab();
                    switch(extension)
                    {
                        case "csv":
                            ReadCSV(streamReader);
                            break;
                        case "xml":
                            ReadXML(streamReader);
                            break;
                        case "json":
                            ReadJson(FileName);
                            break;
                    }
                    dataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
        }

        void ReadCSV(StreamReader streamReader)
        {
            Columns.Clear();
            string[] parameters = streamReader.ReadLine().Split(";");
            foreach (string parameter in parameters)
            {
                Columns.Add(parameter);
            }
            foreach (string column in Columns)
            {
                //Pour ajouter une colonne à notre datagrid
                DataColumn textColumn = new DataColumn();

                textColumn.ColumnName = column;
                dataTable.Columns.Add(textColumn);
            }

            while (!streamReader.EndOfStream)
            {
                string[] test = streamReader.ReadLine().Split(';');
                dataTable.Rows.Add(test);

                //foreach(DataRow textColumn in dataTable.Columns)
                //{
                //    Debug.WriteLine(textColumn[0]);
                //}
                //dataGrid.Items.Add(;
            }
        }

        void ReadXML(StreamReader streamReader)
        {
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(streamReader);
            dataTable = dataSet.Tables[0];
        }

        void ReadJson(string path)
        {
            try
            {
                // Read the JSON file content
                string json = File.ReadAllText(path);

                // Deserialize JSON into a list of dictionaries (one dictionary per row)
                var rows = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);

                if (rows != null && rows.Count > 0)
                {
                    // Add columns to the DataTable based on the keys (e.g., Id, en, fr, ja)
                    foreach (var key in rows[0].Keys)
                    {
                        dataTable.Columns.Add(key);
                    }

                    // Add each row to the DataTable
                    foreach (var row in rows)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        foreach (var key in row.Keys)
                        {
                            dataRow[key] = row[key];  // Populate the cell with the value
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }

                //MessageBox.Show("Data Imported Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error importing JSON: " + ex.Message);
            }
        }

        void ClearTab()
        {
            dataTable.Columns.Clear();
            dataTable.Rows.Clear();

            dataGrid.Columns.Clear();
            dataGrid.ItemsSource = null;
        }
    }

}