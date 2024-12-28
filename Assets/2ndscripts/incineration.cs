using UnityEngine;

public class IncinerateHandler : MonoBehaviour
{
    [Header("Capacity Settings")]
    public DumpingStation dumpingStation; // Reference to the DumpingStation script
    public float capacityIncrease = 100f; // Amount to increase the capacity by

    private void OnTriggerEnter(Collider other)
    {
        // Check if the triggered object has the tag "incinerate"
        if (other.CompareTag("incinerate"))
        {
            if (dumpingStation != null)
            {
                // Increase the dumping station's capacity
                dumpingStation.currentDumpingCapacity += capacityIncrease;

                // Update the UI (if any)
                dumpingStation.uicall(); // Ensure this method exists in your DumpingStation script

                // Log the increase for debugging
                Debug.Log($"Dumping capacity increased by {capacityIncrease}. New capacity: {dumpingStation.currentDumpingCapacity}");

                // Optionally destroy or deactivate the incinerate object
                //Destroy(other.gameObject);
            }
            else
            {
                Debug.LogWarning("DumpingStation reference is not set in IncinerateHandler.");
            }
        }
    }
}
