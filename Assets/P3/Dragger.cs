using UnityEngine;
using UnityEngine.UIElements;

public class Dragger : PointerManipulator
{
    private Vector3 m_Start;
    protected bool m_Active;
    private int m_PointerId;

    public Dragger()
    {
        m_PointerId = -1;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        m_Active = false;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        target.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
    }

    protected void OnPointerDown(PointerDownEvent e)
    {

        if (m_Active)
        {
            // Debug.Log("down");

            e.StopImmediatePropagation();
            return;
        }

        if (CanStartManipulation(e))
        {

            m_Start = e.localPosition;
            m_PointerId = e.pointerId;
            m_Active = true;
            target.CapturePointer(m_PointerId);
            e.StopPropagation();
        }
    }

    protected void OnPointerMove(PointerMoveEvent e)
    {
        if (!m_Active || !target.HasPointerCapture(m_PointerId))
        {
            return;
        }

        Vector2 diff = e.localPosition - m_Start;
        target.style.top = target.layout.y + diff.y;
        target.style.left = target.layout.x + diff.x;

        e.StopPropagation();
    }

    protected void OnPointerUp(PointerUpEvent e)
    {
        if (!m_Active || !target.HasPointerCapture(m_PointerId) || !CanStopManipulation(e))
        {
            return;
        }

        m_Active = false;
        target.ReleaseMouse();
        // m_PointerId = -1;
        e.StopPropagation();
    }
}
