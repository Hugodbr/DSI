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

        Label nameLabel;

        public SaveGameFile(VisualElement savefileRoot, string name) {
            this.savefileRoot = savefileRoot;
            this.name = name;

            nameLabel = savefileRoot.Q<Label>("FileName");

            UpdateUI();
        }

        void UpdateUI(){
            nameLabel.text = name;
        }
    }
}
