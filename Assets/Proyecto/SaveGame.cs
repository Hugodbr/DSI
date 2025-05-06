using System;
using UnityEngine;

namespace ProyectoMain
{
    [Serializable]
    public class SavedGame
    {

        public event Action Cambio;

        [SerializeField]
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != name){
                    name = value;
                    Cambio?.Invoke();
                }
            }
        }

        [SerializeField]
        private DateTime timestamp;
        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }
    }
}
