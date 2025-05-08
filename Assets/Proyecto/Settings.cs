using System;
using UnityEngine;

namespace ProyectoMain
{
    [Serializable]
    public class Settings
    {
        public event Action Cambio;

        [SerializeField]
        private float volume;
        public float Volume
        {
            get { return volume; }
            set
            {
                if (value != volume){
                    volume = value;
                    Cambio?.Invoke();
                }
            }
        }

        [SerializeField]
        private string difficulty;
        public string Difficulty
        {
            get { return difficulty; }
            set
            {
                if (value != difficulty){
                    difficulty = value;
                    Cambio?.Invoke();
                }
            }
        }

        [SerializeField]
        private bool invertedAxis;
        public bool InvertedAxis
        {
            get { return invertedAxis; }
            set
            {
                if (value != invertedAxis){
                    invertedAxis = value;
                    Cambio?.Invoke();
                }
            }
        }

        public Settings(float volume, string difficulty, bool invertedAxis){
            this.volume = volume;
            this.difficulty = difficulty;
            this.invertedAxis = invertedAxis;
        }

        public Settings(Settings other){
            this.volume = other.volume;
            this.difficulty = other.difficulty;
            this.invertedAxis = other.invertedAxis;
        }
    }
}
