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

        List<VisualElement> lveleft = leftgr.Children().ToList();
        List<VisualElement> lveright = rightgr.Children().ToList();

        lveleft.ForEach(elem => elem.AddManipulator(new Dragger()));
        lveright.ForEach(elem => elem.AddManipulator(new Dragger()));

        lveleft.ForEach(elem => elem.AddManipulator(new Lab3Manipulator(ref lveleft)));
        lveright.ForEach(elem => elem.AddManipulator(new Lab3Manipulator(ref lveright)));


    }
}
