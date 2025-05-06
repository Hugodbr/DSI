using UnityEngine;
using System.Collections.Generic;
using System.IO; 


namespace ProyectoMain
{
    public class Basedatos
    {
        public static List<Individuo> getData()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "individuos.json");

            List<Individuo> datos = new List<Individuo>();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                datos = JsonHelper.FromJson<Individuo>(json);
            }
            else
            {
                Debug.Log("No saved data found.");
            }
            return datos;
        }
    }
}