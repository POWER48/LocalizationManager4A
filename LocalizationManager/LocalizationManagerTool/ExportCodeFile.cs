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
using Microsoft.Win32;

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
                    ExportDataTableToXml();
                    break;
                case (int)ComboExport.JSON:
                    ExportDataTableToJson();                   
                    break;
                case (int)ComboExport.CSV:
                    ExportDataTableToCsv();
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


        public void ExportDataTableToJson()
        {
            try
            {
                // Open SaveFileDialog to get the export path
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                    DefaultExt = "json",  // Default file extension
                    FileName = "data.json" // Default file name
                };

                // Show the dialog and check if the user selected a file
                if (saveFileDialog.ShowDialog() == true)
                {
                    // Get the file path chosen by the user
                    string filePath = saveFileDialog.FileName;

                    // Convert DataTable to a list of dictionaries (one per row)
                    var rows = new List<Dictionary<string, object>>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var rowDict = new Dictionary<string, object>();
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            rowDict[column.ColumnName] = row[column];
                        }
                        rows.Add(rowDict);
                    }

                    // Serialize the list of dictionaries to JSON
                    string json = JsonConvert.SerializeObject(rows, Formatting.Indented);

                    // Write the JSON string to the file
                    File.WriteAllText(filePath, json);

                    MessageBox.Show("Exported to JSON successfully!");
                }
                else
                {
                    MessageBox.Show("Export cancelled.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting to JSON: " + ex.Message);
            }
        }

        // Export DataTable to CSV
        public void ExportDataTableToCsv()
        {
            try
            {
                // Open SaveFileDialog to get the export path
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                    DefaultExt = "csv",  // Default file extension
                    FileName = "data.csv" // Default file name
                };

                // Show the dialog and check if the user selected a file
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    // Open the StreamWriter to write to the file
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        // Write the header (column names)
                        var columnNames = dataTable.Columns.Cast<DataColumn>()
                                                           .Select(c => c.ColumnName)
                                                           .ToArray();
                        writer.WriteLine(string.Join(";", columnNames)); // Write the column names as the header

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
                else
                {
                    MessageBox.Show("Export cancelled.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting to CSV: " + ex.Message);
            }
        }

        // Export DataTable to XML
        public void ExportDataTableToXml()
        {
            try
            {
                // Open SaveFileDialog to get the export path
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                    DefaultExt = "xml",  // Default file extension
                    FileName = "data.xml" // Default file name
                };

                // Show the dialog and check if the user selected a file
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    // Write the DataTable to the XML file
                    dataTable.WriteXml(filePath);

                    MessageBox.Show("Exported to XML successfully!");
                }
                else
                {
                    MessageBox.Show("Export cancelled.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting to XML: " + ex.Message);
            }
        }

        // Helper function to escape CSV values
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
