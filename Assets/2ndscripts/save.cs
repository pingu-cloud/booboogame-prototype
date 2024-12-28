using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [Header("References")]
    public DumpingStation dumpingStation; // Reference to the DumpingStation script
    public List<DumpCollection> dumpCollections; // List of DumpCollection scripts
    public GameObject incinerator; // Reference to the incinerator GameObject

    [Header("Reset Values")]
    public float resetDumpingTime = 2f; // Default dumping time
    public float resetCurrentDumpingCapacity = 200f; // Default dumping capacity
    public int resetRatePerDump = 30; // Default rate per dump
    public float resetDumpCollectionCapacity = 100f; // Default capacity for all DumpCollection scripts

    private const string DumpingTimeKey = "DumpingTime";
    private const string CurrentDumpingCapacityKey = "CurrentDumpingCapacity";
    private const string RatePerDumpKey = "RatePerDump";
    private const string DumpCollectionCapacityKey = "DumpCollectionCapacity";
    private const string IncineratorActiveKey = "IncineratorActive";

    private void Start()
    {
        LoadGameState();
    }

    private void OnApplicationQuit()
    {
        SaveGameState();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveGameState();
        }
    }

    private void SaveGameState()
    {
        // Save DumpingStation values
        if (dumpingStation != null)
        {
            PlayerPrefs.SetFloat(DumpingTimeKey, dumpingStation.dumpingTime);
            PlayerPrefs.SetFloat(CurrentDumpingCapacityKey, dumpingStation.currentDumpingCapacity);
            PlayerPrefs.SetInt(RatePerDumpKey, dumpingStation.rateperdump);
        }

        // Save DumpCollection capacities
        if (dumpCollections != null && dumpCollections.Count > 0)
        {
            for (int i = 0; i < dumpCollections.Count; i++)
            {
                string key = $"{DumpCollectionCapacityKey}_{i}";
                PlayerPrefs.SetFloat(key, dumpCollections[i].capacity);
            }
        }

        // Save incinerator active state
        if (incinerator != null)
        {
            PlayerPrefs.SetInt(IncineratorActiveKey, incinerator.activeSelf ? 1 : 0);
        }

        PlayerPrefs.Save();
        Debug.Log("Game state saved.");
    }

    private void LoadGameState()
    {
        // Load DumpingStation values
        if (dumpingStation != null)
        {
            dumpingStation.dumpingTime = PlayerPrefs.GetFloat(DumpingTimeKey, dumpingStation.dumpingTime);
            dumpingStation.currentDumpingCapacity = PlayerPrefs.GetFloat(CurrentDumpingCapacityKey, dumpingStation.currentDumpingCapacity);
            dumpingStation.rateperdump = PlayerPrefs.GetInt(RatePerDumpKey, dumpingStation.rateperdump);
        }

        // Load DumpCollection capacities
        if (dumpCollections != null && dumpCollections.Count > 0)
        {
            for (int i = 0; i < dumpCollections.Count; i++)
            {
                string key = $"{DumpCollectionCapacityKey}_{i}";
                if (PlayerPrefs.HasKey(key))
                {
                    dumpCollections[i].capacity = PlayerPrefs.GetFloat(key);
                }
            }
        }

        // Load incinerator active state
        if (incinerator != null)
        {
            bool isActive = PlayerPrefs.GetInt(IncineratorActiveKey, 0) == 1;
            incinerator.SetActive(isActive);
        }

        Debug.Log("Game state loaded.");
    }

    public void ResetAndRestartGame()
    {
        // Reset DumpingStation values
        if (dumpingStation != null)
        {
            dumpingStation.dumpingTime = resetDumpingTime;
            dumpingStation.currentDumpingCapacity = resetCurrentDumpingCapacity;
            dumpingStation.rateperdump = resetRatePerDump;
        }

        // Reset DumpCollection capacities
        if (dumpCollections != null && dumpCollections.Count > 0)
        {
            foreach (var dumpCollection in dumpCollections)
            {
                dumpCollection.capacity = resetDumpCollectionCapacity;
            }
        }

        // Reset incinerator state
        if (incinerator != null)
        {
            incinerator.SetActive(false);
        }

        // Clear PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("Game state reset to default values. Restarting the game...");

        // Restart the game by reloading the active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
