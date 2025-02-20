using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class Lab3Manipulator : Manipulator
{

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
        target.style.borderBottomColor = Color.white;
        target.style.borderLeftColor = Color.white;
        target.style.borderRightColor = Color.white;
        target.style.borderTopColor = Color.white;
        mev.StopPropagation();
    }
}
