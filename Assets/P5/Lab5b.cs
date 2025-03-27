using UnityEngine.UIElements;
using UnityEngine;

namespace Lab5b_namespace
{
    public class Lab5b : MonoBehaviour
    {
        VisualElement plantilla;
        TextField input_nombre;
        TextField input_apellido;
        Individuo individuoPrueba;

        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            plantilla = root.Q("P4uxml");
            input_nombre = root.Q<TextField>("inputNombre");
            input_apellido = root.Q<TextField>("inputApellido");

            individuoPrueba = new Individuo("Perico", "Palotes");

            Tarjeta tarjetaPrueba = new Tarjeta(plantilla, individuoPrueba);

            // plantilla.RegisterCallback<ClickEvent>(SelectionIndividuo);
            input_nombre.RegisterCallback<ChangeEvent<string>>(CambioNombre);
            input_apellido.RegisterCallback<ChangeEvent<string>>(CambioApellido);

            input_nombre.SetValueWithoutNotify(individuoPrueba.Nombre);
            input_apellido.SetValueWithoutNotify(individuoPrueba.Apellido);
        }

        // void SelectionIndividuo(ClickEvent evt)
        // {
        //     string nombre = plantilla.Q<Label>("Nombre").text;
        //     string apellido = plantilla.Q<Label>("Apellido").text;

        //     input_nombre.SetValueWithoutNotify(nombre);
        //     input_apellido.SetValueWithoutNotify(apellido);
        // }

        void CambioNombre(ChangeEvent<string> evt)
        {
            // Label nombreLabel = plantilla.Q<Label>("Nombre");
            // nombreLabel.text = evt.newValue;
            individuoPrueba.Nombre = evt.newValue;
        }

        void CambioApellido(ChangeEvent<string> evt)
        {
            // Label apellidoLabel = plantilla.Q<Label>("Apellido");
            // apellidoLabel.text = evt.newValue;
            individuoPrueba.Apellido = evt.newValue;
        }
    }
}