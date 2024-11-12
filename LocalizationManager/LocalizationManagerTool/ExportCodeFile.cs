using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Data;

namespace LocalizationManagerTool
{    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum ComboExport
        {
            CSV, 
            JSON,
            XML,                 
            C_SHARP,
            C_PLUS_PLUS
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonExport.IsEnabled = true;
        }

        private void Button_Export(object sender, RoutedEventArgs e)
        {
            
            Button button = (Button)sender;
            ComboExport combo;


            switch (ExportTypeButton.SelectedIndex)
            {
                case (int)ComboExport.XML:
                    ExportXML();
                    break;
                case (int)ComboExport.JSON:
                    ExportJSON();                   
                    break;
                case (int)ComboExport.CSV:
                    ExportCSV();
                    break;
                case (int)ComboExport.C_SHARP:
                    Debug.WriteLine("C#");
                    break;
                case (int)ComboExport.C_PLUS_PLUS:
                    Debug.WriteLine("C++");
                    break;
                default:
                    MessageBox.Show("Error");
                    break;
            };
        }


        private void ExportJSON()
        {
            try
            {
                // Serialize the list to JSON
                string json = JsonConvert.SerializeObject(Columns, Formatting.Indented);

                // Specify the file path
                string filePath = "C://Users/Etudiant1/Desktop/colums.json";

                // Write JSON to file
                File.WriteAllText(filePath, json);

                MessageBox.Show("Exported to JSON successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting to JSON: " + ex.Message);
            }
        }

        private void ExportCSV()
        {
            string filePath = "C://Users/Etudiant1/Desktop/colums.csv";
            try
            {
                // Open the StreamWriter to write to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Write the header (column names)
                    var columnNames = dataTable.Columns.Cast<DataColumn>()
                                                       .Select(c => c.ColumnName)
                                                       .ToArray();
                    writer.WriteLine(string.Join(",", columnNames)); // Write the column names as the header

                    // Write each row of data
                    foreach (DataRow row in dataTable.Rows)
                    {
                        // Write the values for each column in the current row
                        var rowValues = row.ItemArray.Select(value => EscapeCsvValue(value.ToString())).ToArray();
                        writer.WriteLine(string.Join(",", rowValues)); // Write the row values, joined by commas
                    }
                }

                MessageBox.Show("Exported to CSV successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting to CSV: " + ex.Message);
            }
        }

        private void ExportXML()
        {
            try
            {
                // Specify the file path
                string filePath = "C://Users/Etudiant1/Desktop/colums.xml";

                dataTable.WriteXml(filePath);

                MessageBox.Show("Exported to XML successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting to XML: " + ex.Message);
            }
        }
        private string EscapeCsvValue(string value)
        {
            if (string.IsNullOrEmpty(value)) return "\"\""; // Handle empty or null values

            // Escape any double quotes by doubling them
            value = value.Replace("\"", "\"\"");

            // If the value contains commas, quotes, or newlines, wrap it in quotes
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                // Wrap the value in quotes
                value = "\"" + value + "\"";
            }

            return value;
        }
    }

}
