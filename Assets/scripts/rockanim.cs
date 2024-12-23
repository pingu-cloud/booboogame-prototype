using UnityEngine;
using DG.Tweening; // Don't forget to include the DOTween namespace!

public class PopAndBounce : MonoBehaviour
{
    public float popDuration = 0.5f; // Time for the pop-out animation
    public float bounceDuration = 0.3f; // Time for the bounce animation
    public float bounceScale = 1.1f; // How much it overshoots during the bounce
    public float finalScale = 4f; // Final scale of the object
    public ParticleSystem particleEffect; // Reference to the particle system
    public float particleDelay = 0.5f; // Delay after playing the particle before animation starts
    
    

    public void AnimatePopAndBounce()
    {
        // Start with scale zero
        transform.localScale = Vector3.zero;

        // Sequence to chain animations
        Sequence sequence = DOTween.Sequence();

        // Play the particle effect first
        sequence.AppendCallback(() =>
        {
            if (particleEffect != null)
            {
                particleEffect.Play();
            }
        });

        // Add a delay after the particle effect starts
        sequence.AppendInterval(particleDelay);

        // Add the pop-out animation (small to big)
        sequence.Append(transform.DOScale(bounceScale, popDuration).SetEase(Ease.OutBack));

        // Add the bounce animation
        sequence.Append(transform.DOScale(finalScale, bounceDuration).SetEase(Ease.OutBounce));

        // Play the sequence
        sequence.Play();
    }
}
