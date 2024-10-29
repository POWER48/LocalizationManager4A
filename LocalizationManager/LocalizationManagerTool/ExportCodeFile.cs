using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.Xml.Linq;

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


            switch (ExportType.SelectedIndex)
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
            try
            {
                // Specify the file path
                string filePath = "C://Users/Etudiant1/Desktop/colums.csv";

                // Write to CSV file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Write the header (optional)
                    writer.WriteLine("Columns");

                    // Write each column value
                    foreach (var column in Columns)
                    {
                        writer.WriteLine(column);
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

                // Create an XML document
                XElement xml = new XElement("Columns",
                    new XElement("ColumnList",
                        Columns.ConvertAll(column => new XElement("Column", column))
                    )
                );

                // Save the XML document to a file
                xml.Save(filePath);

                MessageBox.Show("Exported to XML successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting to XML: " + ex.Message);
            }
        }
    }

}
