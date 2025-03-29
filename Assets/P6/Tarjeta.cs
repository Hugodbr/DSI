using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


namespace Lab6_namespace
{
    public class Tarjeta
    {
        Individuo mIndividuo;
        VisualElement tarjetaRoot;
        Label nombreLabel;
        Label apellidoLabel;
        VisualElement imagen;

        public Tarjeta(VisualElement tarjetaRoot, Individuo individuo) {
            this.tarjetaRoot = tarjetaRoot;
            this.mIndividuo = individuo;

            nombreLabel = tarjetaRoot.Q<Label>("Nombre");
            apellidoLabel = tarjetaRoot.Q<Label>("Apellido");
            imagen = tarjetaRoot.Q<VisualElement>("image");
            
            tarjetaRoot.userData = mIndividuo;

            UpdateUI();

            mIndividuo.Cambio += UpdateUI;
        }

        void UpdateUI(){
            nombreLabel.text = mIndividuo.Nombre;
            apellidoLabel.text = mIndividuo.Apellido;

            var tex = Resources.Load<Texture2D>(mIndividuo.Imagen);
            imagen.style.backgroundImage = new StyleBackground(tex);
        }
    }
}


