using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class Lab4 : VisualElement
{
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
        Label l = new Label();
        l.text = "Playas: ";
        hierarchy.Add(l);

        items = new List<VisualElement>(5)
        {
            new VisualElement(),
            new VisualElement(),
            new VisualElement(),
            new VisualElement(),
            new VisualElement()
        };

        foreach(VisualElement item in items)
        {
            item.style.width = 50;
            item.style.height = 50;
            item.style.backgroundImage = Resources.Load<Texture2D>("praia-do-forte");

            hierarchy.Add(item);
        }
    }

    private void setDisplay()
    {
        foreach (VisualElement item in items)
        {
            item.style.opacity = 0.5f;
        }

        int i = 0;
        foreach (VisualElement item in items)
        {
            if (i >= Estado) break;
            item.style.opacity = 1;
            i++;
        }
    }

    public new class UxmlFactory : UxmlFactory<Lab4, UxmlTraits> { };

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription myEstado = new UxmlIntAttributeDescription { name = "estado", defaultValue = 0};

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var semaforo = ve as Lab4;
            semaforo.Estado = myEstado.GetValueFromBag(bag, cc);
        }
    }
}
