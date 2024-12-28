using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class money : MonoBehaviour
{
    public TextMeshProUGUI amountText; // Reference to the TextMeshProUGUI component
    private int amount=100; // Integer to store the current amount

    // Start is called before the first frame update
    void Start()
    {
        //amount = 0; // Initialize the amount to 0 or any default value
        UpdateText(); // Update the UI text with the initial value
    }

    // Method to increase the money
    public void increasemoney(int x)
    {
        amount += x; // Increase the amount by x
        UpdateText(); // Update the UI text
    }

    // Method to decrease the money
    public void decreasemoney(int y)
    {
        amount -= y; // Decrease the amount by y
        UpdateText(); // Update the UI text
    }

    // Method to update the TextMeshProUGUI text
    private void UpdateText()
    {
        if (amountText != null)
        {
            amountText.text = $"${amount}"; // Update the text with the current amount
        }
    }
}
