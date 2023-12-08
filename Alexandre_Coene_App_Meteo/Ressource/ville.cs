using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO; // Ajoutez le namespace System.IO pour lire le fichier

namespace Alexandre_Coene_App_Meteo.Service // Définissez le namespace du projet
{
    public class Ville // Définissez la classe Ville
    {
        List<string> ls_ville;
        string filePath = @"Ressource/villeLog.txt"; // Définissez le chemin du fichier
        public Ville()
        {
            ls_ville = new List<string>(); // Créez une liste de chaînes pour stocker les villes
        }

        public List<string> GetVilles() // Renvoie la liste des villes
        {
            return ls_ville; // Renvoie la liste des villes
        }

        public List<string> ReadCitiesFromFile() // Définissez la méthode ReadCitiesFromFile
        {
            if (File.Exists(filePath)) // Vérifiez si le fichier existe
            {
                ls_ville = File.ReadAllLines(filePath).Select(city => city.Trim()).ToList(); // Lire le fichier et ajouter chaque ligne à la liste des villes
            }
            return ls_ville; // Renvoie la liste des villes
        }

        public void WriteCitiesToFile() // Définissez la méthode WriteCitiesToFile
        {
            if (File.Exists(filePath)) // Vérifiez si le fichier existe
            {
                File.WriteAllLines(filePath, ls_ville); // Écrivez la liste des villes dans le fichier
            }
        }

        public void AddVille(string ville) // Définissez la méthode AddVille
        {
            ls_ville.Add(ville); // Ajoutez la ville à la liste
            WriteCitiesToFile(); // Appel à la méthode pour sauvegarder la liste mise à jour           
        }

        public void RemoveVille(string ville)
        {
            ls_ville.Remove(ville); // Supprimez la ville de la liste
            WriteCitiesToFile(); // Appel à la méthode pour sauvegarder la liste mise à jour
        }
    }
}