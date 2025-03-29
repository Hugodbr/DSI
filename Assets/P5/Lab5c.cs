using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;

namespace Lab5c_namespace
{
    public class LabSc : MonoBehaviour
    {
        List<IndividuoC> individuos;
        IndividuoC selecIndividuo;

        VisualElement tarjeta1;
        VisualElement tarjeta2;
        VisualElement tarjeta3;
        VisualElement tarjeta4;

        TextField input_nombre;
        TextField input_apellido;

        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            tarjeta1 = root.Q("Tarjeta1");
            tarjeta2 = root.Q("Tarjeta2"); 
            tarjeta3 = root.Q("Tarjeta3");
            tarjeta4 = root.Q("Tarjeta4");

            input_nombre = root.Q<TextField>("inputNombre");
            input_apellido = root.Q<TextField>("inputApellido");

            individuos = Basedatos.getData();

            VisualElement panelDcha = root.Q("dcha");
            panelDcha.RegisterCallback<ClickEvent>(SeleccionTarjeta);

            input_nombre.RegisterCallback<ChangeEvent<string>>(CambioNombre);
            input_apellido.RegisterCallback<ChangeEvent<string>>(CambioApellido);

            InitializeUI();
        }

        void CambioNombre(ChangeEvent<string> evt)
        {
            selecIndividuo.Nombre = evt.newValue;
        }

        void CambioApellido(ChangeEvent<string> evt)
        {
            selecIndividuo.Apellido = evt.newValue;
        }

        void SeleccionTarjeta(ClickEvent e)
        {
            VisualElement tarjeta = e.target as VisualElement;
            selecIndividuo = tarjeta.userData as IndividuoC;

            input_nombre.SetValueWithoutNotify(selecIndividuo.Nombre);
            input_apellido.SetValueWithoutNotify(selecIndividuo.Apellido);
        }

        void InitializeUI()
        {
            TarjetaC tar1 = new TarjetaC(tarjeta1, individuos[0]);
            TarjetaC tar2 = new TarjetaC(tarjeta2, individuos[1]);
            TarjetaC tar3 = new TarjetaC(tarjeta3, individuos[2]);
            TarjetaC tar4 = new TarjetaC(tarjeta4, individuos[3]);
        }

    }
}