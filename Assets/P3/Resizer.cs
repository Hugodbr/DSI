using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Resizer : PointerManipulator
{
    private Vector3 m_Start;
    protected bool m_Active;
    private int m_PointerId;
    private Vector2 m_StartSize;

    public Resizer()
    {
        m_PointerId = -1;
        activators.Add(new ManipulatorActivationFilter { button = UnityEngine.UIElements.MouseButton.LeftMouse });
        activators.Add(new ManipulatorActivationFilter { button = UnityEngine.UIElements.MouseButton.MiddleMouse });
        m_Active = false;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        target.RegisterCallback<PointerUpEvent>(OnPointerUp);
        target.RegisterCallback<WheelEvent>(OnWheelMove);

    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        target.UnregisterCallback<WheelEvent>(OnWheelMove);
    }

    protected void OnPointerDown(PointerDownEvent e)
    {
        if (m_Active)
        {
            e.StopImmediatePropagation();
            return;
        }

        if (CanStartManipulation(e))
        {
            m_Start = e.localPosition;
            m_StartSize = target.layout.size;
            m_PointerId = e.pointerId;
            m_Active = true;
            target.CapturePointer(m_PointerId);
            e.StopPropagation();
        }
    }

    protected void OnPointerMove(PointerMoveEvent e)
    {
        // if (!m_Active || !target.HasPointerCapture(m_PointerId))
        // {
        //     return;
        // }

        // Vector2 diff = e.localPosition - m_Start;
        // target.style.height = m_StartSize.y + diff.y;
        // target.style.width = m_StartSize.x + diff.x;

        // e.StopPropagation();
    }

        protected void OnWheelMove(WheelEvent e)
    {
        if (!m_Active || !target.HasPointerCapture(m_PointerId))
        {
            return;
        }

        Debug.Log(e.delta);
        Vector2 diff = math.sign(e.delta.y) * new Vector3(1,1,0) - m_Start;
        target.style.height = target.style.height.ConvertTo<float>() * math.sign(e.delta.y) * 1.2f;// m_StartSize.y + diff.y;
        target.style.width = target.style.width.ConvertTo<float>() * math.sign(e.delta.y) * 1.2f;//m_StartSize.x + diff.x;

        e.StopPropagation();
    }

    protected void OnPointerUp(PointerUpEvent e)
    {
        if (!m_Active || !target.HasPointerCapture(m_PointerId) || !CanStopManipulation(e))
        {
            return;
        }

        m_Active = false;
        target.ReleasePointer(m_PointerId);
        m_PointerId = -1;
        e.StopPropagation();
    }
}