using UnityEngine;
using DG.Tweening; // Import DoTween namespace

public class EnableWithBounce : MonoBehaviour
{
    [Header("Animation Settings")]
    public GameObject targetObject; // The GameObject to enable and animate
    public float animationDuration = 0.5f; // Total duration of the animation
    public float bounceScaleMultiplier = 1.2f; // Scale multiplier for the bounce effect
    public Vector3 finalScale = new Vector3(12.6199999f, 6.98000002f, 14.6199999f); // Final target scale

    public void EnableWithBouncyAnimation()
    {
        if (targetObject != null)
        {
            // Ensure the object is enabled and starts with scale zero
            targetObject.SetActive(true);
            targetObject.transform.localScale = Vector3.zero;

            // Calculate the bounce peak scale
            Vector3 bounceScale = finalScale * bounceScaleMultiplier;

            // Create a bouncy scale-up animation
            targetObject.transform.DOScale(bounceScale, animationDuration * 0.5f)
                .SetEase(Ease.OutQuad) // Ease out for the scale-up
                .OnComplete(() =>
                {
                    // Shrink back to the final scale
                    targetObject.transform.DOScale(finalScale, animationDuration * 0.5f)
                        .SetEase(Ease.InBounce); // Add a bouncy effect
                });
        }
        else
        {
            Debug.LogError("Target object is not assigned!");
        }
    }
}
