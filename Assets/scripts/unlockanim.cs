using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening; // Include DOTween namespace

public class ScreenEffectManager : MonoBehaviour
{
    public ParticleSystem particleEffect; // Reference to the particle effect
    public CinemachineVirtualCamera cinemachineCamera; // Reference to the Cinemachine camera
    public Image fadeImage; // Fullscreen UI Image for fading effect
    public float shakeDuration = 0.5f; // Duration of the screen shake
    public float fadeDuration = 1f; // Duration of the fade effect
    public float particlePlayTime = 0.4f; // Time to play the particle effect
    private CinemachineBasicMultiChannelPerlin noise; // Noise module for camera shake
    public CameraSwitcher cameraswitch;
    
    public Transform animatedObject; // The GameObject to animate (set via the Inspector)

    private void Start()
    {
        // Get the noise module from the Cinemachine camera
        if (cinemachineCamera != null)
        {
            noise = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        // Ensure the fade image starts fully transparent
        if (fadeImage != null)
        {
            fadeImage.color = new Color(1, 1, 1, 0); // White color, fully transparent
        }
    }

    
    public void unlockmountains()
    {
        StartCoroutine(HandleEffectSequence());
    }
    private IEnumerator HandleEffectSequence()
    {
        // Step 1: Call VM1Shift
        cameraswitch.VM1Shift();

        // Step 2: Wait for 1 second before calling TriggerEffect
        yield return new WaitForSeconds(1f);

        // Step 3: Trigger the particle and screen effect
        TriggerEffect();
        
    }

    private void TriggerEffect()
    {
        StartCoroutine(PlayEffectSequence());
    }

    private IEnumerator PlayEffectSequence()
    {
        // Step 1: Play the particle effect for 0.4 seconds
        if (particleEffect != null)
        {
            particleEffect.Play();
        }
        yield return new WaitForSeconds(particlePlayTime);

        // Step 2: Start the screen shake
        if (noise != null)
        {
            noise.m_AmplitudeGain = 3f; // Set the shake intensity
            noise.m_FrequencyGain = 3f; // Set the shake frequency
        }

        // Step 3: Fade the screen to white while shaking
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Increase the fade to white
            if (fadeImage != null)
            {
                fadeImage.color = new Color(1, 1, 1, Mathf.Clamp01(elapsedTime / fadeDuration));
            }

            yield return null;
        }

        // Step 4: Hold the white screen for a brief moment
        yield return new WaitForSeconds(0.3f);

        // Step 5: Switch to VM2
        cameraswitch.VM2Shift();

        // Step 6: Wait for 0.8 seconds before starting the animation
        yield return new WaitForSeconds(0.8f);

        // Step 7: Animate the GameObject (bounce and scale)
        if (animatedObject != null)
        {
            // Reset the object's scale
            animatedObject.localScale = Vector3.zero;

            // Debug log to ensure animation starts
            Debug.Log("Starting Scaling Animation");

            // Create a DOTween sequence
            Sequence animationSequence = DOTween.Sequence();

            // First bounce to (0.2, 0.2, 0.2)
            animationSequence.Append(animatedObject.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.4f).SetEase(Ease.OutBack));

            // Then bounce to (0.87, 0.532927036, 1.63999999)
            animationSequence.Append(animatedObject.DOScale(new Vector3(0.87f, 0.532927036f, 1.63999999f), 0.6f).SetEase(Ease.OutBounce));

            // Add OnComplete callback to call MainCameraShift
            animationSequence.OnComplete(() =>
            {
                Debug.Log("Animation Sequence Complete, switching to Main Camera.");
                cameraswitch.MainCameraShift();
            });

            // Play the animation sequence
            animationSequence.Play();
        }
        else
        {
            Debug.LogWarning("No Animated Object Assigned!");
        }

        // Step 8: Fade back to normal and stop the shake
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Fade out from white
            if (fadeImage != null)
            {
                fadeImage.color = new Color(1, 1, 1, 1 - Mathf.Clamp01(elapsedTime / fadeDuration));
            }

            yield return null;
        }

        // Reset the screen shake
        if (noise != null)
        {
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;
        }
    }
}
