using System;
using UnityEngine;

namespace ProyectoMain
{
    [Serializable]
    public class PlayerInfo
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
        private int weapon;
        public int Weapon
        {
            get { return weapon; }
            set
            {
                if (value != weapon){
                    weapon = value;
                    Cambio?.Invoke();
                }
            }
        }

        [SerializeField]
        private int life;
        public int Life
        {
            get { return life; }
            set
            {
                if (value != life){
                    life = value;
                    Cambio?.Invoke();
                }
            }
        }

        public PlayerInfo(string name, int weapon, int life){
            this.name = name;
            this.weapon = weapon;
            this.life = life;
        }

        public PlayerInfo(PlayerInfo other)
        {
            this.name = other.name;
            this.weapon = other.weapon;
            this.life = other.life;
        }

    }
}
