using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UpgradeTruckCapacity : MonoBehaviour
{
    [Header("References")]
    public List<DumpCollection> dumpCollections; // List of DumpCollection scripts
    public money moneyHandler; // Reference to the money script

    [Header("UI Elements")]
    public TextMeshProUGUI capacityText; // Text to display truck capacity upgrades

    [Header("Upgrade Settings")]
    public float capacityIncrease = 30f; // Amount to increase capacity by
    public int upgradeCost = 20; // Cost of upgrading

    private void Start()
    {
        // Initialize UI with current total capacity
        UpdateUI();
    }
    private void OnEnable()
    {
        UpdateUI();
    }

    public void Upgrade()
    {
        if (moneyHandler != null && dumpCollections != null && dumpCollections.Count > 0)
        {
            // Deduct money for the upgrade
            moneyHandler.decreasemoney(upgradeCost);

            // Increase capacity for all DumpCollection scripts
            foreach (var dumpCollection in dumpCollections)
            {
                if (dumpCollection != null)
                {
                    dumpCollection.capacity += capacityIncrease;
                }
            }

            // Update the UI
            UpdateUI();
        }
        else
        {
            Debug.LogError("MoneyHandler or DumpCollections list is missing or empty!");
        }
    }

    private void UpdateUI()
    {
        if (capacityText != null && dumpCollections != null && dumpCollections.Count > 0)
        {
            // Calculate total capacity across all trucks
            float totalCapacity = 0f;
            foreach (var dumpCollection in dumpCollections)
            {
                if (dumpCollection != null)
                {
                    totalCapacity += dumpCollection.capacity;
                }
            }

            // Update UI text
            capacityText.text = $"{totalCapacity}";
        }
        else
        {
            Debug.LogError("CapacityText or DumpCollections list is not assigned properly!");
        }
    }
}
