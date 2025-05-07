using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProyectoMain
{
    /// <summary>
    /// The visual element save file data
    /// </summary>
    public class SaveGameFile
    {
        VisualElement savefileRoot;
        string name;

        Button buttonName;

        public SaveGameFile(VisualElement savefileRoot, string name) {
            this.savefileRoot = savefileRoot;
            this.name = name;
            Debug.Log(this.name);
            buttonName = savefileRoot.Q<Button>("SavedFile");

            UpdateUI();
        }

        void UpdateUI(){
            Debug.Log(name);
            buttonName.text = name;
        }
    }
}
