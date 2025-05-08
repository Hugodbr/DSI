using System;
using UnityEngine;

namespace ProyectoMain
{
    /// <summary>
    /// Data structure for save game
    /// </summary>
    [Serializable]
    public class SaveGame
    {
        [SerializeField]
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; } 
        }

        [SerializeField]
        private bool current;
        public bool Current
        {
            get { return current; }
            set { current = value; } 
        }

        [SerializeField]
        private PlayerInfo playerInfo;
        public PlayerInfo PlayerInfo
        {
            get { return playerInfo; }
            set { playerInfo = value; }
        }

        [SerializeField]
        private Settings settings;
        public Settings Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        public SaveGame(string name, PlayerInfo playerInfo, Settings settings, bool current)
        {
            this.name = name;
            this.current = current;
            this.playerInfo = playerInfo;
            this.settings = settings;
        }

        public SaveGame(SaveGame other)
        {
            this.name = other.name;
            this.current = other.Current;
            this.playerInfo = new PlayerInfo(other.PlayerInfo);
            this.settings = new Settings(other.Settings);
        }
    }
}
