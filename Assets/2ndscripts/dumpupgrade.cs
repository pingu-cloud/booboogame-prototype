using UnityEngine;
using TMPro;

public class UpgradeDumpingStation : MonoBehaviour
{
    [Header("References")]
    public DumpingStation dumpingStation; // Reference to the DumpingStation script
    public money moneyHandler; // Reference to the money script

    [Header("UI Elements")]
    public TextMeshProUGUI rateText; // Text to display rate per dump
    public TextMeshProUGUI capacityText; // Text to display capacity
    public TextMeshProUGUI timeText; // Text to display dumping time

    private void Start()
    {
        // Initialize UI with current values from DumpingStation
        UpdateUI();
    }
    private void OnEnable()
    {
        UpdateUI();
    }

    public void Upgrade()
    {
        if (moneyHandler != null && dumpingStation != null)
        {
            // Deduct money
            moneyHandler.decreasemoney(30);

            // Apply upgrades to dumping station
            dumpingStation.dumpingTime = Mathf.Max(0.2f, dumpingStation.dumpingTime - 0.2f); // Ensure dumpingTime doesn't go below 0.2
            dumpingStation.currentDumpingCapacity += 100f;
            dumpingStation.rateperdump += 4;

            // Update UI
            UpdateUI();
        }
        else
        {
            Debug.LogError("MoneyHandler or DumpingStation reference is missing!");
        }
    }

    private void UpdateUI()
    {
        if (dumpingStation != null)
        {
            // Update UI text with current values
            if (rateText != null)
                rateText.text = $"{dumpingStation.rateperdump}";

            if (capacityText != null)
                capacityText.text = $"{dumpingStation.currentDumpingCapacity}";

            if (timeText != null)
                timeText.text = $"{dumpingStation.dumpingTime:F1}s";
        }
    }
}
