using UnityEngine;
using UnityEngine.Splines;
using System.Collections;

public class DumpCollection : MonoBehaviour
{
    [Header("Locations")]
    public string house; // Tag for the house collider
    public string dump;  // Tag for the dump collider

    [Header("Truck Properties")]
    public float capacity = 100f; // Capacity of the truck

    [Header("Stop Offset")]
    public float stopOffset = 0.5f; // Time delay before stopping at the dump trigger

    private float timeToCollect; // Time taken to collect waste
    private SplineAnimate splineAnimate; // Reference to the SplineAnimate component
    private bool isProcessing = false; // To prevent overlapping coroutines

    public TruckPoolingScript truckPool; // Reference to the pooling script

    private void Awake()
    {
        // Get the SplineAnimate component
        splineAnimate = GetComponent<SplineAnimate>();
        if (splineAnimate == null)
        {
            Debug.LogError("SplineAnimate component not found!");
        }
    }

    private void OnEnable()
    {
        // Play the spline animation when the object is enabled
        splineAnimate?.Play();
        timeToCollect = CalculateCollectionTime(capacity);
    }

    private void OnDisable()
    {
        // Reset the progress
        if (splineAnimate != null)
        {
            splineAnimate.NormalizedTime = 0;
        }

        // Notify the pooling script that the truck is disabled
        if (truckPool != null)
        {
            truckPool.OnTruckDisabled(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isProcessing)
        {
            if (other.CompareTag(house))
            {
                StartCoroutine(CollectDump());
            }
            else if (other.CompareTag(dump))
            {
                // Delegate dumping responsibility to DumpingStation with stop offset
                DumpingStation dumpingStation = other.GetComponent<DumpingStation>();
                if (dumpingStation != null)
                {
                    StartCoroutine(HandleDumpingWithOffset(dumpingStation));
                }
            }
            else if (other.CompareTag("reset"))
            {
                gameObject.SetActive(false);
            }
        }
    }

    private float CalculateCollectionTime(float capacity)
    {
        // Formula: base time 2 seconds + (capacity / 100) * 1
        return 2f + (capacity / 100f) * 1f;
    }

    private IEnumerator CollectDump()
    {
        isProcessing = true;

        // Pause the spline animation
        Debug.Log("Arrived at house. Collecting dump...");
        splineAnimate.Pause();

        // Wait for the calculated collection time
        yield return new WaitForSeconds(timeToCollect);

        // Resume the spline animation
        Debug.Log("Dump collection complete! Resuming animation...");
        splineAnimate.Play();
        isProcessing = false;
    }

    private IEnumerator HandleDumpingWithOffset(DumpingStation dumpingStation)
    {
        isProcessing = true;
        while (dumpingStation.currentDumpingCapacity < capacity)
        {
            Debug.Log("Insufficient capacity. Truck is waiting...");
            yield return new WaitForSeconds(1f); // Recheck every second
        }

        // Proceed with dumping
        Debug.Log("Dumping station has sufficient capacity. Starting dumping process...");
        dumpingStation.HandleDumping(splineAnimate, capacity, stopOffset);
        // Wait for the stop offset before interacting with the dumping station
        Debug.Log($"Truck waiting for stopOffset of {stopOffset} seconds before stopping...");
        yield return new WaitForSeconds(stopOffset);

        // Pause the truck
        splineAnimate.Pause();
        Debug.Log("Truck paused at dumping station.");

        // Wait until there is enough capacity
       

        isProcessing = false;
    }




}
