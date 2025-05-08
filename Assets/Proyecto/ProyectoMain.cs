using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Numerics;
using Unity.VisualScripting;


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


        TextField playerNameUI;
        VisualElement playerLifeUI;
        VisualElement playerWeaponUI;

        Label currentGameLabel;
        SaveGame currentSaveGame;
        VisualElement savedGamesContainer;
        Button saveGameButton;
        Button loadGameButton;
        List<VisualElement> savedFiles;
        string selectedSavedFile;

        Slider settingsVolumeUI;
        DropdownField settingsDifficultyUI;
        Toggle settingsInvertedAxisUI;
        List<string> difficultyChoices;
        /// <summary>
        /// Current player info
        /// </summary>
        PlayerInfo currPlayerInfo;
        /// <summary>
        /// Current saved games
        /// </summary>
        List<SaveGame> savedGames;
        /// <summary>
        /// Current settings
        /// </summary>
        Settings currSettings;

        public static ProyectoMain Instance { get; private set; }

        private void Awake()
        {
            // Debug.Log("Awake Proyecto");
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void OnEnable()
        {
            // Debug.Log("OnEnable Proyecto");
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            VisualElement menu = root.Q<VisualElement>("Menu");

            // Load ans init data
            SaveGame initSave = InitData();

            // Initialize UI elements references
            // Panels
            savedGamesPanel = menu.Q<VisualElement>("SavedGamesPanel");
            settingsPanel = menu.Q<VisualElement>("SettingsPanel");
            playerInfoPanel = menu.Q<VisualElement>("PlayerInfoPanel");
            // Save panel elements
            currentGameLabel = savedGamesPanel.Query<Label>("CurrentData"); // To be updated as a game is saved or loaded
            savedGamesContainer = savedGamesPanel.Query<VisualElement>("SavedGamesContainer");
            // savedFiles = savedGamesContainer.Children().ToList(); // elements of saved files with name and date
            // Settings panel elements
            settingsVolumeUI = settingsPanel.Query<Slider>("SettingsVolume");
            settingsDifficultyUI = settingsPanel.Query<DropdownField>("SettingsDifficulty");
            settingsInvertedAxisUI = settingsPanel.Query<Toggle>("InvertedAxis");
            
            playerNameUI = playerInfoPanel.Query<TextField>("PlayerName");
            playerLifeUI = playerInfoPanel.Q<VisualElement>("PlayerLife");
            playerWeaponUI = playerInfoPanel.Q<VisualElement>("PlayerWeapon");

            saveGameButton = savedGamesPanel.Q<Button>("SaveGameButton");
            loadGameButton = savedGamesPanel.Q<Button>("LoadGameButton");

            // Nav buttons reference
            rightNavigationButton = root.Query<Button>("rightButton").First();
            leftNavigationButton = root.Query<Button>("leftButton").First();

            // Register callbacks
            saveGameButton.RegisterCallback<ClickEvent>(NewSaveFile);
            loadGameButton.RegisterCallback<ClickEvent>(LoadSavedFile);

            settingsVolumeUI.RegisterCallback<ChangeEvent<float>>(ChangeVolume);
            settingsDifficultyUI.RegisterCallback<ChangeEvent<string>>(ChangeDifficulty);

            playerNameUI.RegisterCallback<ChangeEvent<string>>(ChangePlayerName);
            // playerLife.RegisterCallback<ChangeEvent<string>>(ChangePlayerLife);
            // playerWeapon.RegisterCallback<ClickEvent>(ChangeWeapon); //* In wepon script

            rightNavigationButton.RegisterCallback<ClickEvent>(evt => GoToNextPanel(1));
            leftNavigationButton.RegisterCallback<ClickEvent>(evt => GoToNextPanel(-1));

            InitializeUI(initSave);
        }

        /// <summary>
        /// Initialize data from base or create new if none
        /// </summary>
        SaveGame InitData()
        {
            savedGames = Basedatos.GetSavedGames(); // has all saved games names + player info + settings

            if (!savedGames.Any()) { // if there's no saved games
                Debug.Log("No saved games available.");
                selectedSavedFile = ""; // check if the string is empty. If it is, load button won't work

                // Set default data to initialize UI
                currPlayerInfo = new PlayerInfo("Unnamed character", 0, 2);
                currSettings = new Settings(100, "Normal", false);

                currentSaveGame = new SaveGame("No saved games", currPlayerInfo, currSettings, true);

            }
            else {
                SaveGame saveGame = savedGames.Find(save => save.Current == true); // gets the marked as current from other session
                selectedSavedFile = saveGame.Name; // string name of the save file selected by default of the first in the list of saved data

                currentSaveGame = new SaveGame(saveGame);

                // Initialize to update UI
                currPlayerInfo = currentSaveGame.PlayerInfo;
                currSettings = currentSaveGame.Settings;
            }

            return new SaveGame(currentSaveGame);

        }

        void InitializeUI(SaveGame initSave)
        {
            // Debug.Log("InitializeUI");
            InitPanels();

            InitializeSaveUI();
            InitializePlayerUI(initSave);
            InitializeSettingsUI(initSave);
        }

        void InitPanels()
        {
            // Debug.Log("InitPanels");
            panels = new VisualElement[3];
            currentPanel = 0;
            
            panels[currentPanel] = playerInfoPanel; // Initial panel
            panels[1] = savedGamesPanel;
            panels[2] = settingsPanel;

            DeactivateAllPanels();
            ActivatePanel(panels[currentPanel]);
        }

        void InitializeSaveUI() 
        {
            if (savedGames.Any()) // if there're saved games
            { 
                savedGames.ForEach(save => 
                {
                    VisualTreeAsset saveFileTemplate = Resources.Load<VisualTreeAsset>("SavedFileTemplate");
                    VisualElement newSaveFileElement = saveFileTemplate.Instantiate();
                    InitializeFileElement(newSaveFileElement);

                    savedGamesContainer.Insert(0, newSaveFileElement);
                    SavedFileBorderBlack();

                    SaveGame newSaveGame = new SaveGame(
                        save.Name,
                        new PlayerInfo(save.PlayerInfo),
                        new Settings(save.Settings),
                        save.Current
                    );

                    // Debug.Log(save.Settings.InvertedAxis);

                    SaveGameFile saveFile = new SaveGameFile(newSaveFileElement, newSaveGame.Name);

                    if (save.Current) {
                        currentGameLabel.text = save.Name;
                    }
                });
            }
        }

        /// <summary>
        /// Initialize life and weapon
        /// </summary>
        void InitializePlayerUI(SaveGame initSave) 
        {
            UpdatePlayerInfoUI(initSave);
            // playerName.value = playerInfo.Name;
            // value = playerInfo.Life; // TODO: initialize life UI
            //* playerInfo.Weapon UI is initialized by the manipulator
        }

        /// <summary>
        /// Initialize volume, invertedAxis, difficulty
        /// </summary>
        void InitializeSettingsUI(SaveGame initSave) 
        {
            difficultyChoices = new List<string> { "Easy", "Normal", "Hard", "Brasil" };
            settingsDifficultyUI.choices = difficultyChoices;

            UpdateSettingsUI(initSave);
        }

        void UpdateSettingsUI(SaveGame saveGame)
        {
            settingsVolumeUI.value = saveGame.Settings.Volume;
            settingsInvertedAxisUI.value = saveGame.Settings.InvertedAxis;
            settingsDifficultyUI.value = saveGame.Settings.Difficulty;
            // currSettings.Difficulty = loadSave.Settings.Difficulty;
            // currSettings.InvertedAxis = loadSave.Settings.InvertedAxis;
            // currSettings.Volume = loadSave.Settings.Volume;
        }

        void UpdatePlayerInfoUI(SaveGame saveGame)
        {
            playerNameUI.value = saveGame.PlayerInfo.Name;
            // currPlayerInfo.Name = loadSave.PlayerInfo.Name;
            // currPlayerInfo.Weapon = loadSave.PlayerInfo.Weapon;
            // currPlayerInfo.Life = loadSave.PlayerInfo.Life;
        }

        void SelectSavedFile(ClickEvent evt)
        {
            VisualElement saveFile = evt.currentTarget as VisualElement;
            Button btt = saveFile.Q<Button>("SavedFile");
            selectedSavedFile = btt.text;

            SavedFileBorderBlack();
            SavedFileBorderWhite(saveFile);
        }

        /// <summary>
        /// Save a new file with a string name = playerName + Timestamp.
        /// Saves settings and player data
        /// Creates new saveFile visual element template
        /// </summary>
        /// <param name="evt"></param>
        void NewSaveFile(ClickEvent evt)
        {
            VisualTreeAsset saveFileTemplate = Resources.Load<VisualTreeAsset>("SavedFileTemplate");
            VisualElement newSaveFileElement = saveFileTemplate.Instantiate();
            InitializeFileElement(newSaveFileElement);

            savedGamesContainer.Insert(0, newSaveFileElement);
            SavedFileBorderBlack();
            SavedFileBorderWhite(newSaveFileElement);

            SaveGame newSaveGame = new SaveGame(
                currPlayerInfo.Name + " | " + DateTime.Now.ToString(),
                new PlayerInfo(currPlayerInfo),
                new Settings(currSettings),
                true
            );

            savedGames.Add(newSaveGame);
            Basedatos.SaveSavedGames(savedGames);

            if (selectedSavedFile != "") {
                // Updates the current save game loaded
                SaveGame currentSave = savedGames.Find(save => save.Name == currentGameLabel.text);
                currentSave.Current = false;
            }

            UpdateSettingsUI(newSaveGame);
            UpdatePlayerInfoUI(newSaveGame);

            // currPlayerInfo = newSaveGame.PlayerInfo;
            // currSettings = newSaveGame.Settings; ??

            SaveGameFile saveFile = new SaveGameFile(newSaveFileElement, newSaveGame.Name);
            selectedSavedFile = newSaveGame.Name;
            currentGameLabel.text = newSaveGame.Name;

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
            // Debug.Log("load");
            loadSave.Current = true;
            
            // Update current game loaded
            SaveGame currentSave = savedGames.Find(save => save.Name == currentGameLabel.text);
            currentSave.Current = false;

            SaveGame load = new SaveGame(loadSave);
            // currPlayerInfo = load.PlayerInfo;
            // currSettings = load.Settings;

            // Updates data
            UpdateSettingsUI(load);
            UpdatePlayerInfoUI(load);
            
            currentGameLabel.text = selectedSavedFile; // updates the current save data display    

            Basedatos.SaveSavedGames(savedGames);
        }

        void ChangePlayerName(ChangeEvent<string> evt)
        {
            currPlayerInfo.Name = evt.newValue;
        }

        public void ChangeWeapon(int weaponIdx)
        {
            currPlayerInfo.Weapon = weaponIdx;
        }

        public int GetWeapon()
        {
            return currPlayerInfo.Weapon;
        }

        void ChangeVolume(ChangeEvent<float> evt)
        {
            currSettings.Volume = evt.newValue;
        }

        void ChangeDifficulty(ChangeEvent<string> evt)
        {
            currSettings.Difficulty = evt.newValue;
        }

        /// <summary>
        /// Change border color of all file elements
        /// </summary>
        void SavedFileBorderBlack()
        {
            savedGamesContainer.Children().ToList().ForEach(fileElem =>
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
        void SavedFileBorderWhite(VisualElement fileElem)
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

        void InitializeFileElement(VisualElement file)
        {
            file.style.borderBottomWidth = 4;
            file.style.borderTopWidth = 4;
            file.style.borderLeftWidth = 4;
            file.style.borderRightWidth = 4;
            file.RegisterCallback<ClickEvent>(SelectSavedFile);
        }

    }
}
