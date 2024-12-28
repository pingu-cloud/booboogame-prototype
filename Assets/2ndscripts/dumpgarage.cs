using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckPoolingScript : MonoBehaviour
{
    public List<GameObject> truckPool = new List<GameObject>(); // List of pooled trucks
    public float delayBetweenTrucks = 0.2f;
    private bool isNextTruckEnabling = false;

    private void Start()
    {
        // Start enabling trucks at the beginning of the game
        StartCoroutine(EnableTrucksAtStart());
    }

    private IEnumerator EnableTrucksAtStart()
    {
        for (int i = 0; i < truckPool.Count; i++)
        {
            if (truckPool[i] != null)
            {
                truckPool[i].SetActive(true);
                Debug.Log($"Truck {truckPool[i].name} enabled.");
                yield return new WaitForSeconds(delayBetweenTrucks);
            }
        }
    }

    public void OnTruckDisabled(GameObject truck)
    {
        if (truck != null && !truckPool.Contains(truck))
        {
            truckPool.Add(truck);
            Debug.Log($"Truck {truck.name} returned to the pool.");
            StartCoroutine(EnableNextTruck());
        }
    }

    private IEnumerator EnableNextTruck()
    {
        if (isNextTruckEnabling) yield break; // Prevent multiple coroutine instances
        isNextTruckEnabling = true;

        yield return new WaitForSeconds(delayBetweenTrucks);

        if (truckPool.Count > 0)
        {
            GameObject nextTruck = truckPool[0];
            truckPool.RemoveAt(0);
            nextTruck.SetActive(true);
            Debug.Log($"Truck {nextTruck.name} enabled.");
        }

        isNextTruckEnabling = false;
    }
}
