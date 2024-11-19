using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

class XmlImporter
{
    /// <summary>
    /// Imports XML data and converts it into a list of lists of strings for compatibility with CSV-style processing.
    /// </summary>
    /// <param name="filePath">Path to the XML file.</param>
    /// <returns>A list of rows, where each row is a list of string values.</returns>
    public static List<List<string>> ImportXml(string filePath)
    {
        var result = new List<List<string>>();

        // Check if file exists
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        // Load XML document
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(filePath);

        // Get all "TraductionDataTable" nodes
        var nodes = xmlDoc.SelectNodes("/DocumentElement/TraductionDataTable");
        if (nodes == null || nodes.Count == 0)
            throw new InvalidDataException("No data found in XML file.");

        // Extract headers from the first node
        var headers = new List<string>();
        var firstNode = nodes[0];
        foreach (XmlNode child in firstNode.ChildNodes)
        {
            headers.Add(child.Name);
        }
        result.Add(headers);

        // Extract rows
        foreach (XmlNode node in nodes)
        {
            var row = new List<string>();
            foreach (XmlNode child in node.ChildNodes)
            {
                row.Add(child.InnerText ?? string.Empty);
            }
            result.Add(row);
        }

        return result;
    }

    /// <summary>
    /// Prints the data for verification, row by row.
    /// </summary>
    /// <param name="data">Nested list of strings representing the data.</param>
#if !UNITY_2017_1_OR_NEWER
    public static void PrintData(List<List<string>> data)
    {
        foreach (var row in data)
        {
            Console.WriteLine(string.Join(", ", row));
        }
    }
#else
    public static void PrintData(List<List<string>> data)
    {
        foreach (var row in data)
        {
            UnityEngine.Debug.Log(string.Join(", ", row));
        }
    }
#endif
}

// Uncomment the following code to test the XML importer:
// class Program
// {
//     static void Main()
//     {
//         string filePath = "path/to/your/xmlfile.xml"; // Replace with your XML file path

//         try
//         {
//             var data = XmlImporter.ImportXml(filePath);
//             Console.WriteLine("Data imported:");
//             XmlImporter.PrintData(data);
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Error: {ex.Message}");
//         }
//     }
// }
