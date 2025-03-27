using UnityEngine;
using UnityEngine.UIElements;

public class Lab5a : MonoBehaviour
{
    VisualElement plantilla;

    TextField input_nombre;
    TextField input_apellido;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        plantilla = root.Q("P4uxml");
        input_nombre = root.Q<TextField>("inputNombre");
        input_apellido = root.Q<TextField>("inputApellido");

        plantilla.RegisterCallback<ClickEvent>(SeleccionIndividuo);
        input_nombre.RegisterCallback<ChangeEvent<string>>(CambioNombre);
        input_apellido.RegisterCallback<ChangeEvent<string>>(CambioApellido);
    }

    void SeleccionIndividuo(ClickEvent evt)
    {
        string nombre = plantilla.Q<Label>("Nombre").text;
        string apellido = plantilla.Q<Label>("Apellido").text;
        
        Debug.Log(nombre);

        input_nombre.SetValueWithoutNotify(nombre);
        input_apellido.SetValueWithoutNotify(apellido);
    }

    void CambioNombre(ChangeEvent<string> evt)
    {
        Label nombreLabel = plantilla.Q<Label>("Nombre");
        nombreLabel.text = evt.newValue;
    }

    void CambioApellido(ChangeEvent<string> evt)
    {
        Label apellidoLabel = plantilla.Q<Label>("Apellido");
        apellidoLabel.text = evt.newValue;
    }
}
