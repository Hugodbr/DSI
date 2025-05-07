using UnityEngine;
using System.Collections.Generic;
using System.IO; 


namespace ProyectoMain
{
    public class Basedatos
    {
        private static readonly string SavedGamesPath = Path.Combine(Application.persistentDataPath, "saved_games.json");

        public static List<SaveGame> GetSavedGames()
        {
            if (File.Exists(SavedGamesPath))
            {
                string json = File.ReadAllText(SavedGamesPath);
                return JsonHelper.FromJson<SaveGame>(json);
            }

            // empty list
            return new List<SaveGame>();
        }

        public static void SaveSavedGames(List<SaveGame> savedGames)
        {
            string json = JsonHelper.ToJson(savedGames, true);
            File.WriteAllText(SavedGamesPath, json);
            Debug.Log($"Saved to: {SavedGamesPath}");
        }

    }
}