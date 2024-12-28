using System;
using UnityEngine;
using TMPro; // For TextMeshPro

public class OfflineEarningsCalculator : MonoBehaviour
{
    [Header("References")]
    public money moneyHandler; // Reference to the money handler script
    public DumpCollection car; // Reference to the car script
    public DumpingStation dumpingStation; // Reference to the dumping station script
    public TextMeshProUGUI offlineEarningsText; // Text to display offline earnings message

    [Header("Base Earnings Settings")]
    public float baseEarning = 30f; // Default earning for one loop
    public float baseTime = 27f; // Default time for one loop in seconds
    public int noOfCars = 1; // Number of cars

    private void Start()
    {
        CalculateOfflineEarnings();
    }

    private void OnApplicationQuit()
    {
        SaveLastOnlineTime();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) // When the app goes to the background
        {
            SaveLastOnlineTime();
        }
    }

    private void SaveLastOnlineTime()
    {
        // Save the current time as the last online time
        PlayerPrefs.SetString("LastOnlineTime", DateTime.Now.ToString());
        PlayerPrefs.Save();
        Debug.Log($"Last online time saved: {DateTime.Now}");
    }

    private void CalculateOfflineEarnings()
    {
        // Get the last time the player was online
        string lastOnlineTimeString = PlayerPrefs.GetString("LastOnlineTime", DateTime.Now.ToString());
        DateTime lastOnlineTime = DateTime.Parse(lastOnlineTimeString);

        // Calculate the time passed since the player was last online
        TimeSpan timePassed = DateTime.Now - lastOnlineTime;
        float totalSecondsPassed = (float)timePassed.TotalSeconds;

        // Ensure references are not null
        if (car == null || dumpingStation == null || moneyHandler == null)
        {
            Debug.LogError("Missing references in OfflineEarningsCalculator!");
            return;
        }

        // Get properties from the car and dump station
        float carCapacity = car.capacity; // Capacity of the car
        float dumpingTime = dumpingStation.dumpingTime; // Time required for dumping

        // Calculate the actual time per loop
        float actualTimePerLoop = baseTime * (100f / carCapacity) + dumpingTime;

        // Calculate loops completed by all cars in offline time
        float loopsCompleted = totalSecondsPassed / actualTimePerLoop;

        // Calculate total earnings
        float totalEarnings = loopsCompleted * baseEarning * noOfCars;

        // Update the player's money
        moneyHandler.increasemoney(Mathf.FloorToInt(totalEarnings));

        // Show offline earnings message
        DisplayOfflineEarningsMessage(totalSecondsPassed, totalEarnings);

        Debug.Log($"Offline earnings: {totalEarnings}. Time passed: {totalSecondsPassed}s. Loops completed: {loopsCompleted}");
    }

    private void DisplayOfflineEarningsMessage(float timeGone, float earnings)
    {
        if (offlineEarningsText != null)
        {
            // Convert timeGone to hours, minutes, seconds format
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeGone);
            string timeGoneFormatted = $"{timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s";

            // Update the text with the offline earnings message
            offlineEarningsText.text = $"You were gone for {timeGoneFormatted} and earned ${Mathf.FloorToInt(earnings)}!";
        }
        else
        {
            Debug.LogWarning("Offline earnings text reference is not assigned.");
        }
    }
}
