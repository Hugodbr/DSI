using UnityEngine.UIElements;

namespace Lab5c_namespace
{
    public class TarjetaC
    {
        IndividuoC mIndividuo;
        VisualElement tarjetaRoot;

        Label nombreLabel;
        Label apellidoLabel;

        public TarjetaC(VisualElement tarjetaRoot, IndividuoC individuo)
        {
            this.tarjetaRoot = tarjetaRoot;
            this.mIndividuo = individuo;

            nombreLabel = tarjetaRoot.Q<Label>("Nombre");
            apellidoLabel = tarjetaRoot.Q<Label>("Apellido");
            tarjetaRoot.userData = mIndividuo;

            tarjetaRoot
                .Query(className: "tarjeta")
                .Descendents<VisualElement>()
                .ForEach(elem => elem.pickingMode = PickingMode.Ignore);

            UpdateUI();
            mIndividuo.Cambio += UpdateUI;
        }

        void UpdateUI()
        {
            nombreLabel.text = mIndividuo.Nombre;
            apellidoLabel.text = mIndividuo.Apellido;
        }
    }
}