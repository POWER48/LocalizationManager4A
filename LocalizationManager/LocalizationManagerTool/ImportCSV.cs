using System;
using System.Collections.Generic;
using System.IO;

class CsvImporter
{
    /// <summary>
    /// Lit un fichier CSV et retourne une double liste où chaque ligne est une liste de chaînes.
    /// </summary>
    /// <param name="filePath">Chemin du fichier CSV.</param>
    /// <returns>Liste imbriquée représentant les lignes du fichier CSV.</returns>
    public static List<List<string>> ImportCsv(string filePath)
    {
        var result = new List<List<string>>();

        using (var reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (line == null) continue;

                // Séparer les valeurs par des virgules
                var values = new List<string>(line.Split(','));

                // Ajouter la liste de valeurs à la liste principale
                result.Add(values);
            }
        }

        return result;
    }

    /// <summary>
    /// Affiche les données pour vérification.
    /// </summary>
    /// <param name="data">Double liste contenant les données.</param>
    public static void PrintData(List<List<string>> data)
    {
        foreach (var row in data)
        {
            Console.WriteLine(string.Join(", ", row));
        }
    }
}

class ImportCSV
{
    //static void Main()
    //{
    //    string filePath = "C:/Users/Etudiant1/Downloads/data.csv"; // Chemin du fichier CSV

    //    try
    //    {
    //        var data = CsvImporter.ImportCsv(filePath);
    //        Console.WriteLine("Données importées :");
    //        CsvImporter.PrintData(data);
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Erreur : {ex.Message}");
    //    }
    //}
}