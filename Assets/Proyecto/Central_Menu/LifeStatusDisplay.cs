using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.VisualElement;
using UnityEngine.UIElements;

public class LifeStatusDisplay : VisualElement
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

    public LifeStatusDisplay()
    {
        items = new List<VisualElement>(5)
        {
            new VisualElement(),
            new VisualElement(),
            new VisualElement(),
            new VisualElement(),
            new VisualElement()
        };

        foreach (VisualElement item in items)
        {
            item.style.width = 50;
            item.style.height = 50;
            item.style.backgroundImage = Resources.Load<Texture2D>("LifeHeart");

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

    public new class UxmlFactory : UxmlFactory<LifeStatusDisplay, UxmlTraits> { };

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription myEstado = new UxmlIntAttributeDescription { name = "life points", defaultValue = 0 };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var semaforo = ve as LifeStatusDisplay;
            semaforo.Estado = myEstado.GetValueFromBag(bag, cc);
        }
    }
}
