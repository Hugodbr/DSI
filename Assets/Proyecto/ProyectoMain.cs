using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO; 


namespace ProyectoMain
{
    public class ProyectoMain : MonoBehaviour
    {
        VisualElement playerInfoPanel;
        VisualElement savedGamesPanel;
        VisualElement settingsPanel;

        TextField playerName;
        VisualElement playerLife;
        VisualElement playerWeapon;

        VisualElement savedGamesContainer;
        Button saveGameButton;
        Button loadGameButton;
        List<VisualElement> savedFiles;
        string selectedSavedFile;

        Slider settingsVolume;
        DropdownField settingsDifficulty;
        Toggle settingsInvertAxis;

        /// <summary>
        /// Current player info
        /// </summary>
        PlayerInfo playerInfo;
        /// <summary>
        /// Current saved games
        /// </summary>
        List<SaveGame> savedGames;
        /// <summary>
        /// Current settings
        /// </summary>
        Settings settings;

        // VisualElement botonCrear;
        // VisualElement botonGuardar;
        // Toggle invertAxis;
        // VisualElement contenedor_dcha;
        // TextField saveGameName;
        // Individuo individuoSelec;
        // List<VisualElement> imagenes;

        // List<Individuo> listIndividuos;

        public static ProyectoMain Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            // Load ans init data
            InitData();

            // Initialize UI elements references
            savedGamesPanel = root.Q<VisualElement>("SavedGamesPanel");
            settingsPanel = root.Q<VisualElement>("SettingsPanel");
            playerInfoPanel = root.Q<VisualElement>("PlayerInfoPanel");

            savedGamesContainer = savedGamesPanel.Q<VisualElement>("SavedGamesContainer"); // ! update or assert child of savedGamesPanel
            savedFiles = savedGamesContainer.Children().ToList(); // elements of saved files with name and date

            settingsVolume = settingsPanel.Q<Slider>("SettingsVolume"); // ! update or assert child of settingsPanel
            settingsDifficulty = settingsPanel.Q<DropdownField>("SettingsDifficulty"); // ! update or assert child of settingsPanel

            playerName = playerInfoPanel.Q<TextField>("PlayerName"); // ! update or assert child of playerInfoPanel
            playerLife = playerInfoPanel.Q<VisualElement>("PlayerLife"); // ! update or assert child of playerInfoPanel // VisualElement?
            playerWeapon = playerInfoPanel.Q<VisualElement>("PlayerWeapon"); // ! update or assert child of playerInfoPanel // VisualElement?

            saveGameButton = savedGamesPanel.Q<Button>("SaveGameButton"); // ! update or assert child of savedGamesPanel
            loadGameButton = savedGamesPanel.Q<Button>("LoadGameButton"); // ! update or assert child of savedGamesPanel

            // LoadImages(ref imagenes);
            // imagenSelec = imagenes.First().name;
            // CajasBordeRojo(imagenes.First());

            // Register callbacks
            savedFiles.ForEach(file => file.RegisterCallback<ClickEvent>(SelectSavedFile));
            saveGameButton.RegisterCallback<ClickEvent>(NewSaveFile);
            loadGameButton.RegisterCallback<ClickEvent>(LoadSavedFile);

            settingsVolume.RegisterCallback<ChangeEvent<float>>(ChangeVolume); // TODO float?
            settingsDifficulty.RegisterCallback<ChangeEvent<string>>(ChangeDifficulty); // TODO 

            playerName.RegisterCallback<ChangeEvent<string>>(ChangePlayerName);
            // playerLife.RegisterCallback<ChangeEvent<string>>(ChangePlayerLife);
            // playerWeapon.RegisterCallback<ClickEvent>(ChangeWeapon);

            // botonCrear.RegisterCallback<ClickEvent>(NuevaTarjeta);
            // botonGuardar.RegisterCallback<ClickEvent>(SaveIndividuosToFile);
            // input_nombre.RegisterCallback<ChangeEvent<string>>(CambioNombre);
            // input_apellido.RegisterCallback<ChangeEvent<string>>(CambioApellido);
            // contenedor_dcha.RegisterCallback<ClickEvent>(SeleccionTarjeta);
            // imagenes.ForEach(caja => caja.RegisterCallback<ClickEvent>(SeleccionImagen));

            InitializeUI();
        }

        /// <summary>
        /// Initialize data from base or create new if none
        /// </summary>
        void InitData()
        {
            savedGames = Basedatos.GetSavedGames(); // has all saved games names + player info + settings

            if (!savedGames.Any()) { // if there's no saved games
                Debug.Log("No saved games available.");
                selectedSavedFile = ""; // check if the string is empty. If it is, load button won't work

                // Set default data to initialize UI
                playerInfo = new PlayerInfo("Unnamed player", 0, 2);
                settings = new Settings(100, "Normal", false);
            }
            else {
                selectedSavedFile = savedFiles.First().name; // string name of the save file selected by default

                // Initialize to update UI
                playerInfo = savedGames.First().PlayerInfo;
                settings = savedGames.First().Settings;
            }
        }

        void InitializeUI()
        {
            InitializeSaveUI();
            InitializePlayerUI();
            InitializeSettingsUI();
        }

        void InitializeSaveUI() 
        {
            if (savedGames.Any()) // if there're saved games
            { 
                savedGames.ForEach(indv => 
                {
                    VisualTreeAsset saveFileTemplate = Resources.Load<VisualTreeAsset>("SaveFile");
                    VisualElement newSaveFileElement = saveFileTemplate.Instantiate();

                    savedGamesContainer.Add(newSaveFileElement);
                    SaveCtnrBorderBlack();
                    SaveCntrBorderWhite(newSaveFileElement);

                    SaveGame newSaveGame = new SaveGame(
                        playerInfo.Name + " | " + DateTime.Now.ToString(),
                        playerInfo,
                        settings
                    );

                    SaveGameFile saveFile = new SaveGameFile(newSaveFileElement, newSaveGame.Name);
                });
            }
        }

        /// <summary>
        /// Initialize life and weapon
        /// </summary>
        void InitializePlayerUI() 
        {
            playerName.value = playerInfo.Name;
            // value = playerInfo.Life; // TODO: initialize life UI
            //* playerInfo.Weapon UI is initialized by the manipulator
        }

        /// <summary>
        /// Initialize volume, invertedAxis, difficulty
        /// </summary>
        void InitializeSettingsUI() 
        {
            settingsVolume.value = settings.Volume;
            settingsInvertAxis.value = settings.InvertedAxis;

            List<string> difficultyChoices = new List<string> { "Easy", "Normal", "Hard", "Brasil" };
            settingsDifficulty = new DropdownField(difficultyChoices, 1); // 1 = default to "Normal"
            settingsDifficulty.value = settings.Difficulty;
        }

        void SelectSavedFile(ClickEvent evt)
        {
            VisualElement saveFile = evt.currentTarget as VisualElement;
            selectedSavedFile = saveFile.name;

            SaveCtnrBorderBlack();
            SaveCntrBorderWhite(saveFile);
        }

        /// <summary>
        /// Save a new file with a string name = playerName + Timestamp.
        /// Saves settings and player data
        /// Creates new saveFile visual element template
        /// </summary>
        /// <param name="evt"></param>
        void NewSaveFile(ClickEvent evt)
        {
            VisualTreeAsset saveFileTemplate = Resources.Load<VisualTreeAsset>("SaveFile");
            VisualElement newSaveFileElement = saveFileTemplate.Instantiate();

            savedGamesContainer.Add(newSaveFileElement);
            SaveCtnrBorderBlack();
            SaveCntrBorderWhite(newSaveFileElement);

            SaveGame newSaveGame = new SaveGame(
                playerInfo.Name + " | " + DateTime.Now.ToString(),
                playerInfo,
                settings
            );

            SaveGameFile saveFile = new SaveGameFile(newSaveFileElement, newSaveGame.Name);
            selectedSavedFile = newSaveGame.Name;

            savedGames.Add(newSaveGame);
            JsonHelper.ToJson(savedGames, true); // save list to json
        }

        /// <summary>
        /// Load the saved file with name selectedSavedFile.
        /// Updates all data from UI to data state (playerInfo, settings)
        /// </summary>
        /// <param name="evt"></param>
        void LoadSavedFile(ClickEvent evt)
        {
            
        }

        // void SeleccionTarjeta(ClickEvent e)
        // {
        //     VisualElement mTarjeta = e.target as VisualElement;
        //     individuoSelec = mTarjeta.userData as Individuo;

        //     if (individuoSelec != null)
        //     {
        //         input_nombre.SetValueWithoutNotify(individuoSelec.Nombre);
        //         input_apellido.SetValueWithoutNotify(individuoSelec.Apellido);
        //         toggleModificar.value = true;

        //         TarjetasBordeNegro();
        //         TarjetasBordeBlanco(mTarjeta);
        //     }
        // }

        void ChangePlayerName(ChangeEvent<string> evt)
        {
            playerInfo.Name = evt.newValue;
        }

        public void ChangeWeapon(int weaponIdx)
        {
            playerInfo.Weapon = weaponIdx;
        }

        public int GetWeapon()
        {
            return playerInfo.Weapon;
        }

        void ChangeVolume(ChangeEvent<float> evt)
        {
            settings.Volume = evt.newValue;
        }

        void ChangeDifficulty(ChangeEvent<string> evt)
        {
            settings.Difficulty = evt.newValue;
        }

        // ! Use as examplo if feature to delete multiple save datas
        // void CambioNombre(ChangeEvent<string> evt){
        //     if (toggleModificar.value){
        //         individuoSelec.Nombre = evt.newValue;
        //     }
        // }

        void SaveIndividuosToFile(ClickEvent evt)
        {
            string json = "";// JsonHelper.ToJson(listIndividuos, true);
            string filePath = Path.Combine(Application.persistentDataPath, "individuos.json");

            if (File.Exists(filePath)) {
                File.Delete(filePath); 
                Debug.Log("File deleted.");
            }

            File.WriteAllText(filePath, json);
            Debug.Log($"Saved to: {filePath}");
        }

        /// <summary>
        /// Change border color of all file elements
        /// </summary>
        void SaveCtnrBorderBlack()
        {
            savedFiles.ForEach(fileElem =>
                {
                    fileElem.style.borderBottomColor = Color.black;
                    fileElem.style.borderRightColor  = Color.black;
                    fileElem.style.borderTopColor    = Color.black;
                    fileElem.style.borderLeftColor   = Color.black;
                });
        }

        /// <summary>
        /// Highlights the selected file element
        /// </summary>
        /// <param name="fileElem"></param>
        void SaveCntrBorderWhite(VisualElement fileElem)
        {
            fileElem.style.borderBottomColor = Color.white;
            fileElem.style.borderRightColor  = Color.white;
            fileElem.style.borderTopColor    = Color.white;
            fileElem.style.borderLeftColor   = Color.white;
        }
    }
}
