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
                    ExportDataTableToCSFiles();
                    break;
                case (int)ComboExport.C_PLUS_PLUS:
                    ExportDataTableToCppFiles();
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
                            writer.WriteLine(string.Join(";", rowValues)); // Write the row values, joined by commas
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


        public void ExportDataTableToCppFiles()
        {
            try
            {
                // Ouvrir le SaveFileDialog pour le fichier .h
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Header Files (*.h)|*.h",
                    DefaultExt = "h",  // Extension par défaut
                    FileName = "Dialog.h" // Nom de fichier par défaut
                };

                // Montrer la boîte de dialogue pour sauvegarder le fichier .h
                if (saveFileDialog.ShowDialog() == true)
                {
                    string headerFilePath = saveFileDialog.FileName;

                    // Créer et écrire dans le fichier .h
                    using (StreamWriter headerWriter = new StreamWriter(headerFilePath))
                    {
                        headerWriter.WriteLine("#ifndef DIALOG_H");
                        headerWriter.WriteLine("#define DIALOG_H");
                        headerWriter.WriteLine();
                        headerWriter.WriteLine("#include <vector>");
                        headerWriter.WriteLine("#include <string>");
                        headerWriter.WriteLine();
                        headerWriter.WriteLine("class Dialog {");
                        headerWriter.WriteLine("public:");

                        // Déclaration du membre vector de vector de string
                        headerWriter.WriteLine("    std::vector<std::vector<std::string>> data;");

                        headerWriter.WriteLine("    Dialog();");
                        headerWriter.WriteLine("    void printData();");
                        headerWriter.WriteLine("};");
                        headerWriter.WriteLine();
                        headerWriter.WriteLine("#endif // DIALOG_H");
                    }

                    // Maintenant, ouvrir le SaveFileDialog pour le fichier .cpp
                    saveFileDialog.Filter = "C++ Files (*.cpp)|*.cpp";
                    saveFileDialog.DefaultExt = "cpp";  // Extension par défaut
                    saveFileDialog.FileName = "Dialog.cpp"; // Nom de fichier par défaut

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string cppFilePath = saveFileDialog.FileName;

                        // Créer et écrire dans le fichier .cpp
                        using (StreamWriter cppWriter = new StreamWriter(cppFilePath))
                        {
                            cppWriter.WriteLine("#include \"Dialog.h\"");
                            cppWriter.WriteLine();
                            cppWriter.WriteLine("Dialog::Dialog() {");

                            // Initialisation des données
                            cppWriter.WriteLine("    data = {");
                            foreach (DataRow row in dataTable.Rows)
                            {
                                cppWriter.WriteLine("        {");
                                for (int i = 0; i < dataTable.Columns.Count; i++)
                                {
                                    var value = row[i].ToString().Replace("\"", "\\\""); // Échapper les guillemets
                                    cppWriter.WriteLine($"            \"{value}\",");
                                }
                                cppWriter.WriteLine("        },");
                            }
                            cppWriter.WriteLine("    };");
                            cppWriter.WriteLine("}");

                            // Méthode pour afficher les données
                            cppWriter.WriteLine();
                            cppWriter.WriteLine("void Dialog::printData() {");
                            cppWriter.WriteLine("    for (const auto& row : data) {");
                            cppWriter.WriteLine("        for (const auto& col : row) {");
                            cppWriter.WriteLine("            std::cout << col << \" \";");
                            cppWriter.WriteLine("        }");
                            cppWriter.WriteLine("        std::cout << std::endl;");
                            cppWriter.WriteLine("    }");
                            cppWriter.WriteLine("}");
                        }

                        MessageBox.Show("Les fichiers .h et .cpp ont été générés avec succès.");
                    }
                    else
                    {
                        MessageBox.Show("Exportation annulée pour le fichier .cpp.");
                    }
                }
                else
                {
                    MessageBox.Show("Exportation annulée pour le fichier .h.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'exportation : " + ex.Message);
            }
        }

        private void ExportDataTableToCSFiles()
        {
            try
            {
                // Ouvrir le SaveFileDialog pour le fichier .cs
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "C# Files (*.cs)|*.cs",
                    DefaultExt = "cs",  // Extension par défaut
                    FileName = "Dialog.cs" // Nom de fichier par défaut
                };

                // Montrer la boîte de dialogue pour sauvegarder le fichier .cs
                if (saveFileDialog.ShowDialog() == true)
                {
                    string csFilePath = saveFileDialog.FileName;

                    // Créer et écrire dans le fichier .cs
                    using (StreamWriter csWriter = new StreamWriter(csFilePath))
                    {
                        // Début du fichier C#
                        csWriter.WriteLine("using System;");
                        csWriter.WriteLine("using System.Collections.Generic;");
                        csWriter.WriteLine();
                        csWriter.WriteLine("public class Dialog");
                        csWriter.WriteLine("{");

                        // Déclaration du membre List de List de string
                        csWriter.WriteLine("    private List<List<string>> data;");
                        csWriter.WriteLine();

                        // Constructeur
                        csWriter.WriteLine("    public Dialog()");
                        csWriter.WriteLine("    {");
                        csWriter.WriteLine("        data = new List<List<string>>();");
                        csWriter.WriteLine();

                        // Écriture des données de DataTable dans le fichier C#
                        csWriter.WriteLine("        // Initialisation des données");
                        csWriter.WriteLine("        data.AddRange(new List<List<string>>");
                        csWriter.WriteLine("        {");

                        // Loop through each row in the DataTable and output as C# code
                        foreach (DataRow row in dataTable.Rows)
                        {
                            csWriter.WriteLine("            new List<string> {");
                            for (int i = 0; i < dataTable.Columns.Count; i++)
                            {
                                var value = row[i].ToString().Replace("\"", "\\\""); // Escape quotes
                                csWriter.WriteLine($"                \"{value}\",{(i < dataTable.Columns.Count - 1 ? "" : "")}");
                            }
                            csWriter.WriteLine("            },");
                        }

                        csWriter.WriteLine("        });");
                        csWriter.WriteLine("    }");
                        csWriter.WriteLine();

                        // Méthode pour afficher les données
                        csWriter.WriteLine("    public void PrintData()");
                        csWriter.WriteLine("    {");
                        csWriter.WriteLine("        foreach (var row in data)");
                        csWriter.WriteLine("        {");
                        csWriter.WriteLine("            foreach (var col in row)");
                        csWriter.WriteLine("            {");
                        csWriter.WriteLine("                Console.Write(col + \" \");");
                        csWriter.WriteLine("            }");
                        csWriter.WriteLine("            Console.WriteLine();");
                        csWriter.WriteLine("        }");
                        csWriter.WriteLine("    }");
                        csWriter.WriteLine("}");

                        MessageBox.Show("Le fichier .cs a été généré avec succès.");
                    }
                }
                else
                {
                    MessageBox.Show("Exportation annulée.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'exportation : " + ex.Message);
            }
        }
    }
}

