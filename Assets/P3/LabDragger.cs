using UnityEngine;
using UnityEngine.UIElements;

public class Lab3Dragger : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        VisualElement item = root.Q("item");

        // item.AddManipulator(new Lab3Manipulator());
        // item.AddManipulator(new Resizer());
        item.AddManipulator(new Dragger());
    }
}
