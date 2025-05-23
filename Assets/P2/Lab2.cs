using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Lab2 : MonoBehaviour
{
    VisualElement grid_azul;
    VisualElement grid_verde;
    VisualElement grid_naranja;

    VisualElement p_azul;
    VisualElement p_verde;
    VisualElement p_naranja;

    VisualElement header;

    private void OnEnable()
    {
        UIDocument uidoc = GetComponent<UIDocument>();
        VisualElement rootve = uidoc.rootVisualElement;

        grid_azul = rootve.Q("gridAzul");
        grid_verde = rootve.Q("gridVerde");
        grid_naranja = rootve.Q("gridNaranja");

        header = rootve.Q("header");

        p_azul = header.Q("P_azul");
        p_verde = header.Q("P_verde");
        p_naranja = header.Q("P_naranja");

        p_azul.RegisterCallback<MouseDownEvent>(evt => {
            Undisplay();
            grid_azul.style.display = DisplayStyle.Flex;
        });
        p_verde.RegisterCallback<MouseDownEvent>(evt => {
            Undisplay();
            grid_verde.style.display = DisplayStyle.Flex;
        });
        p_naranja.RegisterCallback<MouseDownEvent>(evt => {
            Undisplay();
            grid_naranja.style.display = DisplayStyle.Flex;
        });

        List<VisualElement> items_naranjas = rootve.Query(className: "slider").ToList();
        VisualElement slider = items_naranjas.First();
        Debug.Log(slider);
        slider.style.backgroundColor = Color.blue;

    }

    private void Undisplay()
    {
        grid_azul.style.display = DisplayStyle.None;
        grid_verde.style.display = DisplayStyle.None;
        grid_naranja.style.display = DisplayStyle.None;
    }


}
