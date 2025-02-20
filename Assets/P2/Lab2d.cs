using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Lab2d : MonoBehaviour
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

        List<VisualElement> botones_azules = rootve.Query(className: "bazul").ToList();
        List<VisualElement> botones_verdes = rootve.Query(className: "bverde").ToList();
        List<VisualElement> botones_naranjas = rootve.Query(className: "bnaranja").ToList();

        foreach(VisualElement b in botones_azules)
        {
            b.RegisterCallback<MouseDownEvent>(evt => {
                SetPageAzul();
            });
        }        
        foreach(VisualElement b in botones_verdes)
        {
            b.RegisterCallback<MouseDownEvent>(evt => {
                SetPageVerde();
            });
        }        
        foreach(VisualElement b in botones_naranjas)
        {
            b.RegisterCallback<MouseDownEvent>(evt => {
                SetPageNaranja();
            });
        }


        grid_azul = rootve.Q("menuAzul");
        grid_verde = rootve.Q("menuVerde");
        grid_naranja = rootve.Q("menuNaranja");

    }

    private void Undisplay()
    {
        grid_azul.style.display = DisplayStyle.None;
        grid_verde.style.display = DisplayStyle.None;
        grid_naranja.style.display = DisplayStyle.None;
    }

    private void SetPageAzul()
    {
        Undisplay();
        grid_azul.style.display = DisplayStyle.Flex;
    }    
    private void SetPageVerde()
    {
        Undisplay();
        grid_verde.style.display = DisplayStyle.Flex;
    }    
    private void SetPageNaranja()
    {
        Undisplay();
        grid_naranja.style.display = DisplayStyle.Flex;
    }
}
