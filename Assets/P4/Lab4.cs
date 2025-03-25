using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Lab4 : VisualElement
{
    public new class UxmlFactory : UxmlFactory<Lab4> { };

    List<VisualElement> items;

    int estado;

    public int Estado
    {
        get => estado;
        set
        {
            estado = value;
            setDisplay();
        }
    }

    public Lab4()
    {
        items = new List<VisualElement>(5);

        foreach(VisualElement item in items)
        {
            item.style.width = 100;
            item.style.height = 100;
            item.style.backgroundColor = Color.gray;

            //item.AddToClassList("blabla");

            hierarchy.Add(item);
        }
    }

    private void setDisplay()
    {

    }
}
