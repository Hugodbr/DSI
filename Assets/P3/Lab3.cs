using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Lab3 : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        VisualElement leftPanel = root.Q("leftPanel");
        VisualElement rightPanel = root.Q("rightPanel");

        VisualElement leftgr = leftPanel.Q("lgrid");
        VisualElement rightgr = rightPanel.Q("rgrid");

        // leftPanel.AddManipulator(new Lab3Manipulator());
        // rightPanel.AddManipulator(new Lab3Manipulator());
        // leftPanel.AddManipulator(new Dragger());
        // rightPanel.AddManipulator(new Dragger());

        List<VisualElement> lveleft = leftgr.Children().ToList();
        List<VisualElement> lveright = rightgr.Children().ToList();

        lveleft.ForEach(elem => elem.AddManipulator(new Lab3Manipulator()));
        lveright.ForEach(elem => elem.AddManipulator(new Lab3Manipulator()));
        // lveleft.ForEach(elem => elem.AddManipulator(new Dragger()));
        // lveright.ForEach(elem => elem.AddManipulator(new Dragger()));
        lveleft.ForEach(elem => elem.AddManipulator(new Resizer()));
        lveright.ForEach(elem => elem.AddManipulator(new Resizer()));
    }
}
