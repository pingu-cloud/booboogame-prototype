using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera mainCamera; // Reference to the main Cinemachine Virtual Camera
    public CinemachineVirtualCamera vm1; // Reference to the first alternate Cinemachine Virtual Camera
    public CinemachineVirtualCamera vm2; // Reference to the second alternate Cinemachine Virtual Camera

    private void Start()
    {
        // Ensure the main camera starts with the highest priority
        MainCameraShift();
    }

    public void MainCameraShift()
    {
        Debug.Log("Switching to Main Camera");
        mainCamera.Priority = 10;
        vm1.Priority = 5;
        vm2.Priority = 5;
    }

    public void VM1Shift()
    {
        Debug.Log("Switching to VM1");
        mainCamera.Priority = 5;
        vm1.Priority = 10;
        vm2.Priority = 5;
    }

    public void VM2Shift()
    {
        Debug.Log("Switching to VM2");
        mainCamera.Priority = 5;
        vm1.Priority = 5;
        vm2.Priority = 10;
    }
}
