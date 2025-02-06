using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Lab2 : MonoBehaviour
{

    private void OnEnable()
    {
        UIDocument uidoc = GetComponent<UIDocument>();
        VisualElement rootve = uidoc.rootVisualElement;

        UQueryBuilder<VisualElement> builder = new UQueryBuilder<VisualElement>(rootve);
        List<VisualElement> lista_ve = builder.ToList();

        lista_ve.ForEach(elem => Debug.Log(elem.name));

        VisualElement grid = rootve.Q("grid");
        VisualElement item3 = grid.Q("item3");

        item3.style.display = DisplayStyle.None;
    }
}
