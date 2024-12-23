using UnityEngine;
using UnityEngine.UI;

public class Unlock : MonoBehaviour
{
    public Image progressBar; // Reference to the progress bar Image
    public float stayDuration = 1f; // Time the player must stay before starting the countdown
    public float countdownDuration = 1.5f; // Countdown duration for upgrade
    public RockLevelManager rockManager; // Reference to the rock level manager

    private float stayTimer = 0f; // Tracks how long the player has stayed in the trigger
    private float countdownTimer = 0f; // Tracks the countdown for upgrade
    private bool playerInZone = false; // Is the player in the trigger zone?
    private bool countdownStarted = false; // Has the countdown started?

    private void Start()
    {
        if (progressBar != null)
        {
            progressBar.fillAmount = 0f; // Ensure the progress bar starts empty
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Check if the object is the player
        if (other.CompareTag("Player"))
        {
            playerInZone = true;

            // Increment the stay timer while the player is in the zone
            stayTimer += Time.deltaTime;

            // Start the countdown if the player has stayed for 1 second and it's not started yet
            if (stayTimer >= stayDuration && !countdownStarted)
            {
                countdownStarted = true;
                countdownTimer = countdownDuration; // Initialize countdown timer
            }

            // If the countdown has started, decrease the countdown timer
            if (countdownStarted)
            {
                countdownTimer -= Time.deltaTime;

                // Update the progress bar fill amount
                if (progressBar != null)
                {
                    progressBar.fillAmount = 1 - (countdownTimer / countdownDuration);
                }

                // If countdown reaches zero, call the manager to level up and reset timers
                if (countdownTimer <= 0f)
                {
                    if (rockManager != null)
                    {
                        rockManager.UnlockMountain();
                    }
                    ResetTimers();
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Reset everything if the player exits the trigger zone
        if (other.CompareTag("Player"))
        {
            ResetTimers();
        }
    }

    private void ResetTimers()
    {
        stayTimer = 0f;
        countdownTimer = 0f;
        playerInZone = false;
        countdownStarted = false;

        // Reset the progress bar
        if (progressBar != null)
        {
            progressBar.fillAmount = 0f;
        }
    }
}
