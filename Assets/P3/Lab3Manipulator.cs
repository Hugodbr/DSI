using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


public class Lab3Manipulator : MouseManipulator
{
    List<VisualElement> allElems;

    public Lab3Manipulator(ref List<VisualElement> _allElems)
    {
        allElems = _allElems;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.RightMouse });
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
    }

    private void OnMouseDown(MouseDownEvent mev)
    {

        if (CanStartManipulation(mev)) 
        {
            resetAll();
            
            target.style.borderBottomColor = Color.blue;
            target.style.borderLeftColor = Color.blue;
            target.style.borderRightColor = Color.blue;
            target.style.borderTopColor = Color.blue;
            mev.StopPropagation();
        }
    }

    private void resetAll()
    {
        allElems.ForEach(vl => {
            vl.style.borderBottomColor = new Color(255.0f, 49.0f, 226.0f, 1.0f);
            vl.style.borderLeftColor = new Color(255.0f, 49.0f, 226.0f, 1.0f);
            vl.style.borderRightColor = new Color(255.0f, 49.0f, 226.0f, 1.0f);
            vl.style.borderTopColor = new Color(255.0f, 49.0f, 226.0f, 1.0f);
        });
    }
}
