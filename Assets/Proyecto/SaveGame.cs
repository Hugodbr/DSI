using System;
using UnityEngine;

namespace ProyectoMain
{
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

        public SaveGame(string name, PlayerInfo playerInfo, Settings settings)
        {
            this.name = name;
            this.playerInfo = playerInfo;
            this.settings = settings;
        }
    }
}
