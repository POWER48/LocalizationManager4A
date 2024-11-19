using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

class JsonImporter
{
    /// <summary>
    /// Imports JSON data and converts it into a list of lists of strings for compatibility with CSV-style processing.
    /// </summary>
    /// <param name="filePath">Path to the JSON file.</param>
    /// <returns>A list of rows, where each row is a list of string values.</returns>
    public static List<List<string>> ImportJson(string filePath)
    {
        var result = new List<List<string>>();

        // Check if file exists
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        // Read and deserialize JSON file
        string json = File.ReadAllText(filePath);
        var records = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);

        if (records == null)
            throw new InvalidDataException("Failed to parse JSON file.");

        // Extract headers
        var headers = new List<string>(records[0].Keys);
        result.Add(headers);

        // Extract rows
        foreach (var record in records)
        {
            var row = new List<string>();
            foreach (var header in headers)
            {
                row.Add(record[header]?.ToString() ?? string.Empty);
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

// Uncomment the following code to test the JSON importer:
// class Program
// {
//     static void Main()
//     {
//         string filePath = "path/to/your/jsonfile.json"; // Replace with your JSON file path

//         try
//         {
//             var data = JsonImporter.ImportJson(filePath);
//             Console.WriteLine("Data imported:");
//             JsonImporter.PrintData(data);
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Error: {ex.Message}");
//         }
//     }
// }