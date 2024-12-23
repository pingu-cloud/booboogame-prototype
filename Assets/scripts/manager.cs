using UnityEngine;

public class RockLevelManager : MonoBehaviour
{
    public GameObject[] rockLevels; // Array of rock GameObjects (rock level 1 to 4)
    public GameObject mountain; // Mountain GameObject
    public ScaleBounceEffect money; // Money animation effect

    private int currentLevel;
    private int costToLevelUp = 4; // Base cost to level up rocks
    private bool mountainUnlocked;

    private void Start()
    {
        // Load data from GameDataManager
        var gameData = GameDataManager.Instance.gameData;
        currentLevel = gameData.currentLevel;
        mountainUnlocked = gameData.mountainUnlocked;

        // Initialize rocks
        foreach (var rock in rockLevels)
        {
            rock.SetActive(false);
        }

        // Activate the current rock level without playing animation
        if (currentLevel > 0 && currentLevel <= rockLevels.Length)
        {
            rockLevels[currentLevel - 1].SetActive(true);
        }

        // Handle mountain
        if (mountainUnlocked)
        {
            mountain.SetActive(true);
            mountain.transform.localScale = new Vector3(0.87f, 0.532927036f, 1.63999999f); // Directly scale to final size
        }
    }

    public void LevelUpRock()
    {
        int playerMoney = GameDataManager.Instance.gameData.playerMoney;

        // Check if the player has enough money
        if (playerMoney < costToLevelUp)
        {
            Debug.Log("Not enough money to level up!");
            return;
        }

        // Deduct money with ScaleBounce animation
        DeductMoney(costToLevelUp);

        // Deactivate the current rock if there is one
        if (currentLevel > 0 && currentLevel <= rockLevels.Length)
        {
            rockLevels[currentLevel - 1].SetActive(false);
        }

        // Activate the next rock level and play animation
        if (currentLevel < rockLevels.Length)
        {
            currentLevel++;
            rockLevels[currentLevel - 1].SetActive(true);

            // Play the animation only when leveling up
            var pop = rockLevels[currentLevel - 1].GetComponent<PopAndBounce>();
            if (pop != null)
            {
                pop.AnimatePopAndBounce();
            }

            Debug.Log($"Upgraded to rock level {currentLevel}");
            costToLevelUp += 4; // Increase the cost for the next level
            SaveGameState();
        }
        else
        {
            Debug.Log("Maximum level reached.");
        }
    }

    public void UnlockMountain()
    {
        int playerMoney = GameDataManager.Instance.gameData.playerMoney;

        // Check if the player has enough money
        if (playerMoney < 14) // Example unlock cost
        {
            Debug.Log("Not enough money to unlock the mountain!");
            return;
        }

        // Deduct money with ScaleBounce animation
        DeductMoney(14);

        // Unlock the mountain
        if (!mountainUnlocked)
        {
            mountainUnlocked = true;
            mountain.SetActive(true);

            // Play mountain unlock animation
            var mount = mountain.GetComponent<ScreenEffectManager>();
            if (mount != null)
            {
                mount.unlockmountains();
            }

            //money.ScaleBounce(14); // Example animation
            SaveGameState();
        }
    }

    private void DeductMoney(int amount)
    {
        int playerMoney = GameDataManager.Instance.gameData.playerMoney;

        // Deduct money
        playerMoney -= amount;
        GameDataManager.Instance.gameData.playerMoney = playerMoney;

        // Play ScaleBounce animation
        if (money != null)
        {
            money.ScaleBounce(amount);
        }

        // Update money UI
        GameDataManager.Instance.UpdateMoneyText();

        // Save the updated state
        GameDataManager.Instance.SaveGame();
    }

    private void SaveGameState()
    {
        var gameData = GameDataManager.Instance.gameData;
        gameData.currentLevel = currentLevel;
        gameData.mountainUnlocked = mountainUnlocked;
        GameDataManager.Instance.SaveGame();
    }
}
