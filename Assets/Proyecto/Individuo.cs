using System;
using UnityEngine;


namespace ProyectoMain
{
    [Serializable]
    public class Individuo
    {
        public event Action Cambio;

        [SerializeField]
        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set
            {
                if (value != nombre){
                    nombre = value;
                    Cambio?.Invoke();
                }
            }
        }

        [SerializeField]
        private string apellido;
        public string Apellido
        {
            get { return apellido; }
            set
            {
                if (value != apellido){
                    apellido = value;
                    Cambio?.Invoke();
                }
            }
        }

        [SerializeField]
        private string imagen;
        public string Imagen
        {
            get { return imagen; }
            set
            {
                if (value != imagen){
                    imagen = value;
                    Cambio?.Invoke();
                }
            }
        }

        public Individuo(string nombre, string apellido, string imagen){
            this.nombre = nombre;
            this.apellido = apellido;
            this.imagen = imagen;
        }
    }
}