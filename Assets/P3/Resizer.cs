using UnityEngine.UIElements;

public class Resizer : PointerManipulator
{
    protected bool m_Active;
    private int m_PointerId;
    private bool m_IsLeftMouse; 

    public Resizer()
    {
        m_PointerId = -1;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.MiddleMouse });
        m_Active = false;
        m_IsLeftMouse = false;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        target.RegisterCallback<PointerUpEvent>(OnPointerUp);
        target.RegisterCallback<WheelEvent>(OnWheelMove);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
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
            m_PointerId = e.pointerId;
            m_Active = true;
            m_IsLeftMouse = e.button == (int)MouseButton.LeftMouse;
            target.CapturePointer(m_PointerId);
            e.StopPropagation();
        }
    }

    protected void OnWheelMove(WheelEvent e)
    {
        if (!m_Active || !target.HasPointerCapture(m_PointerId) || !m_IsLeftMouse)
        {
            return;
        }

        float currentWidth = target.style.width.value.value > 0 ? target.style.width.value.value : target.layout.width;
        float currentHeight = target.style.height.value.value > 0 ? target.style.height.value.value : target.layout.height;

        float scaleFactor = 1.1f;
        float delta = e.delta.y;

        if (delta < 0)
        {
            target.style.width = currentWidth * scaleFactor;
            target.style.height = currentHeight * scaleFactor;
        }
        else if (delta > 0)
        {
            target.style.width = currentWidth / scaleFactor;
            target.style.height = currentHeight / scaleFactor;
        }

        e.StopPropagation();
    }

    protected void OnPointerUp(PointerUpEvent e)
    {
        if (!m_Active || !target.HasPointerCapture(m_PointerId) || !CanStopManipulation(e))
        {
            return;
        }

        m_Active = false;
        m_IsLeftMouse = false;
        target.ReleasePointer(m_PointerId);
        m_PointerId = -1;
        e.StopPropagation();
    }
}