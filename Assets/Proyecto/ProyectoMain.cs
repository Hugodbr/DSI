using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Numerics;


namespace ProyectoMain
{
    public class ProyectoMain : MonoBehaviour
    {
        VisualElement playerInfoPanel;
        VisualElement savedGamesPanel;
        VisualElement settingsPanel;
        Button rightNavigationButton;
        Button leftNavigationButton;
        // For cycling over panels with button event
        VisualElement[] panels;
        int currentPanel;


        TextField playerName;
        VisualElement playerLife;
        VisualElement playerWeapon;

        Label currentGameLabel;
        SaveGame currentSaveGame;
        VisualElement savedGamesContainer;
        Button saveGameButton;
        Button loadGameButton;
        List<VisualElement> savedFiles;
        string selectedSavedFile;

        Slider settingsVolume;
        DropdownField settingsDifficulty;
        Toggle settingsInvertedAxis;
        List<string> difficultyChoices;
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
            VisualElement menu = root.Q<VisualElement>("Menu");

            // Load ans init data
            InitData();

            // Initialize UI elements references
            savedGamesPanel = menu.Q<VisualElement>("SavedGamesPanel");
            settingsPanel = menu.Q<VisualElement>("SettingsPanel");
            playerInfoPanel = menu.Q<VisualElement>("PlayerInfoPanel");

            savedGamesContainer = savedGamesPanel.Query<VisualElement>("SavedGamesContainer");
            currentGameLabel = savedGamesPanel.Query<Label>("CurrentData"); // To be updated as a game is saved or loaded
            savedFiles = savedGamesContainer.Children().ToList(); // elements of saved files with name and date

            settingsVolume = settingsPanel.Query<Slider>("SettingsVolume");
            settingsDifficulty = settingsPanel.Query<DropdownField>("SettingsDifficulty");
            settingsInvertedAxis = settingsPanel.Query<Toggle>("InvertedAxis");

            playerName = playerInfoPanel.Query<TextField>("PlayerName"); // ! update or assert child of playerInfoPanel
            playerLife = playerInfoPanel.Q<VisualElement>("PlayerLife"); // ! update or assert child of playerInfoPanel // VisualElement?
            playerWeapon = playerInfoPanel.Q<VisualElement>("PlayerWeapon"); // ! update or assert child of playerInfoPanel // VisualElement?

            saveGameButton = savedGamesPanel.Q<Button>("SaveGameButton"); // ! update or assert child of savedGamesPanel
            loadGameButton = savedGamesPanel.Q<Button>("LoadGameButton"); // ! update or assert child of savedGamesPanel

            // Nav buttons reference
            rightNavigationButton = root.Query<Button>("rightNavButton").First();
            leftNavigationButton = root.Query<Button>("leftNavButton").First();


            // LoadImages(ref imagenes);
            // imagenSelec = imagenes.First().name;
            // CajasBordeRojo(imagenes.First());

            // Register callbacks
            savedFiles.ForEach(file => file.RegisterCallback<ClickEvent>(SelectSavedFile));
            saveGameButton.RegisterCallback<ClickEvent>(NewSaveFile);
            loadGameButton.RegisterCallback<ClickEvent>(LoadSavedFile);

            settingsVolume.RegisterCallback<ChangeEvent<float>>(ChangeVolume);
            settingsDifficulty.RegisterCallback<ChangeEvent<string>>(ChangeDifficulty);

            playerName.RegisterCallback<ChangeEvent<string>>(ChangePlayerName);
            // playerLife.RegisterCallback<ChangeEvent<string>>(ChangePlayerLife);
            // playerWeapon.RegisterCallback<ClickEvent>(ChangeWeapon); //* In wepon script

            rightNavigationButton.RegisterCallback<ClickEvent>(evt => GoToNextPanel(1));
            leftNavigationButton.RegisterCallback<ClickEvent>(evt => GoToNextPanel(-1));





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
                // currentSaveGame = 
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
            InitPanels();

            InitializeSaveUI();
            InitializePlayerUI();
            InitializeSettingsUI();
        }

        void InitPanels()
        {
            currentPanel = 0;

            panels[currentPanel] = playerInfoPanel; // Initial panel
            panels[1] = savedGamesPanel;
            panels[2] = settingsPanel;
        }

        void InitializeSaveUI() 
        {
            if (savedGames.Any()) // if there're saved games
            { 
                savedGames.ForEach(save => 
                {
                    VisualTreeAsset saveFileTemplate = Resources.Load<VisualTreeAsset>("SaveFile");
                    VisualElement newSaveFileElement = saveFileTemplate.Instantiate();

                    savedGamesContainer.Add(newSaveFileElement);
                    SaveCtnrBorderBlack();

                    SaveGame newSaveGame = new SaveGame(
                        save.Name,
                        save.PlayerInfo,
                        save.Settings,
                        save.Current
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
            settingsInvertedAxis.value = settings.InvertedAxis;

            difficultyChoices = new List<string> { "Easy", "Normal", "Hard", "Brasil" };
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
                settings,
                true
            );

            // Updates the current save game loaded
            currentSaveGame.Current = false;
            currentSaveGame = newSaveGame;

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
            if (selectedSavedFile == "") // if no saved game, does nothing
                return;

            SaveGame loadSave = savedGames.Find(save => save.Name == selectedSavedFile); // gets the save with the selected name

            // Updates data
            playerInfo = loadSave.PlayerInfo;
            settings = loadSave.Settings;
            
            // Update current game loaded
            currentSaveGame.Current = false;
            currentSaveGame = loadSave;
            currentGameLabel.text = selectedSavedFile; // updates the current save data display
        }

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
        // void Cambio(ChangeEvent<string> evt){
        //     if (toggleModificar.value){
        //         individuoSelec.Nombre = evt.newValue;
        //     }
        // }

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

        void DeactivateAllPanels()
        {
            foreach (var panel in panels)
            {
                panel.style.display = DisplayStyle.None;
            }
        }

        void ActivatePanel(VisualElement panel)
        {
            panel.style.display = DisplayStyle.Flex;
        }

        /// <summary>
        /// Cycle over panels
        /// </summary>
        /// <param name="direction"></param>
        void GoToNextPanel(int direction)
        {
            currentPanel += direction;

            if (currentPanel > 2) currentPanel = 0;
            else if (currentPanel < 0) currentPanel = 2;

            DeactivateAllPanels();
            ActivatePanel(panels[currentPanel]);
        }

    }
}
