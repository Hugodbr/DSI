using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Lab3Manipulator : MouseManipulator
{
    private List<VisualElement> allElems;

    public Lab3Manipulator(ref List<VisualElement> _allElems)
    {
        allElems = _allElems;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
        target.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseEnterEvent>(OnMouseEnter);
        target.UnregisterCallback<MouseLeaveEvent>(OnMouseLeave);
    }

    private void OnMouseEnter(MouseEnterEvent evt)
    {
        ResetAll();

        target.style.borderBottomColor = Color.blue;
        target.style.borderLeftColor = Color.blue;
        target.style.borderRightColor = Color.blue;
        target.style.borderTopColor = Color.blue;

        evt.StopPropagation();
    }

    private void OnMouseLeave(MouseLeaveEvent evt)
    {
        target.style.borderBottomColor = Color.white;
        target.style.borderLeftColor = Color.white;
        target.style.borderRightColor = Color.white;
        target.style.borderTopColor = Color.white;

        evt.StopPropagation();
    }

    private void ResetAll()
    {
        allElems.ForEach(vl =>
        {
            vl.style.borderBottomColor = Color.white;
            vl.style.borderLeftColor = Color.white;
            vl.style.borderRightColor = Color.white;
            vl.style.borderTopColor = Color.white;
        });
    }
}