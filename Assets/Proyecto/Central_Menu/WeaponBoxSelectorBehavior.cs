using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProyectoMain {
    public class WeaponBoxSelectorBehavior : MonoBehaviour
    {
        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            root.Query(className: "weaponSelectorBox").First().AddManipulator(new WeaponSelectorManipulator());
        }
    }
}
