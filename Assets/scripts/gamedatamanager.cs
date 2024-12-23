using UnityEngine;
using TMPro; // For TextMeshPro
using System.IO;
using UnityEngine.SceneManagement; // For reloading the scene

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    public TextMeshProUGUI moneyText; // Reference to the TextMeshProUGUI displaying the player's total money
    private string saveFilePath;

    // Game data structure
    [System.Serializable]
    public class GameData
    {
        public int currentLevel = 0; // Rock level
        public int playerMoney = 100; // Player's remaining money (default is 100)
        public bool mountainUnlocked = false; // Mountain unlock state
    }

    public GameData gameData = new GameData();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        saveFilePath = Application.persistentDataPath + "/gameData.json";

        LoadGame(); // Load the game data on start

        UpdateMoneyText(); // Update money UI with the loaded value
    }

    private void OnApplicationQuit()
    {
        // Save the current value displayed in the moneyText field
        if (moneyText != null && int.TryParse(moneyText.text, out int money))
        {
            gameData.playerMoney = money;
        }

        SaveGame(); // Save the game data when the application quits
    }

    public void SaveGame()
    {
        try
        {
            string jsonData = JsonUtility.ToJson(gameData, true);

            // Write data to a temporary file first to ensure atomic saving
            string tempFilePath = saveFilePath + ".tmp";
            File.WriteAllText(tempFilePath, jsonData);

            // Replace the main save file with the temporary file
            if (File.Exists(saveFilePath)) File.Delete(saveFilePath);
            File.Move(tempFilePath, saveFilePath);

            Debug.Log($"Game saved: {jsonData}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to save game: {ex.Message}");
        }
    }

    public void LoadGame()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                string jsonData = File.ReadAllText(saveFilePath);
                gameData = JsonUtility.FromJson<GameData>(jsonData);

                Debug.Log($"Game loaded: {jsonData}");
            }
            else
            {
                Debug.LogWarning("Save file not found. Starting with default values.");
            }

            UpdateMoneyText();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load game: {ex.Message}");
        }
    }

    public void ResetGameData()
    {
        // Reset game data to default values
        gameData.currentLevel = 0;
        gameData.playerMoney = 100;
        gameData.mountainUnlocked = false;

        SaveGame(); // Save the reset data to the file

        Debug.Log("Game data has been reset. Restarting the game...");

        // Restart the game by reloading the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = gameData.playerMoney.ToString();
        }
    }
}
