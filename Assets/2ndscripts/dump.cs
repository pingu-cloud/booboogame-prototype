using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

public class DumpingStation : MonoBehaviour
{
    [Header("Dumping Settings")]
    public float dumpingTime = 2.0f; // Time to dump all capacity
    public float currentDumpingCapacity = 1000f; // Current capacity of the dumping station
    public TextMeshProUGUI textbox;
    public money moneyhandle;
    private bool isDumping = false; // Ensures only one truck dumps at a time
    public int rateperdump;
    private void Start()
    {
        uicall();
    }
    public void HandleDumping(SplineAnimate truckSplineAnimate, float truckCapacity, float stopOffset)
    {
        if (truckSplineAnimate != null)
        {
            StartCoroutine(HandleDump(truckSplineAnimate, truckCapacity, stopOffset));
        }
    }

    private IEnumerator HandleDump(SplineAnimate truckSplineAnimate, float truckCapacity, float stopOffset)
    {
        // Wait for the stop offset before pausing the truck
        Debug.Log($"Truck waiting for stopOffset of {stopOffset} seconds.");
        yield return new WaitForSeconds(stopOffset);

        // Pause the truck's spline animation
        truckSplineAnimate.Pause();
        Debug.Log("Truck paused at dumping station.");

        // Wait until the dumping station is available
        while (isDumping || currentDumpingCapacity < truckCapacity)
        {
            Debug.Log("Dumping station is busy or has insufficient capacity. Truck is waiting...");
            yield return new WaitForSeconds(1f); // Check every 1 second
        }

        // Mark the station as busy
        isDumping = true;

        // Start dumping
        Debug.Log("Dumping started.");
        yield return new WaitForSeconds(dumpingTime);

        // Reduce the dumping station's capacity
        currentDumpingCapacity -= truckCapacity;
        Debug.Log($"Dumping complete. Remaining capacity: {currentDumpingCapacity}");

        // Update the capacity display
        uicall();

        // Resume the truck's spline animation
        truckSplineAnimate.Play();
        Debug.Log("Truck resumed after dumping.");
        moneyhandle.increasemoney(rateperdump);
        // Mark the station as free
        isDumping = false;
    }
    public void uicall()
    {
        if (textbox != null)
        {
            textbox.text = currentDumpingCapacity.ToString();
        }
    }
}
