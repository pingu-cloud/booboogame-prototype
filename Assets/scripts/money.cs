using UnityEngine;
using TMPro; // For TextMeshPro
using DG.Tweening; // For DOTween animations

public class ScaleBounceEffect : MonoBehaviour
{
    public float scaleFactor = 1.2f; // How much the object scales up
    public float animationDuration = 0.2f; // Duration of each scale-up and scale-down animation
    public TextMeshProUGUI moneyText; // Reference to the TextMeshPro text displaying the money

    private int currentMoney; // Internal counter to track the money value

    private void Start()
    {
        // Initialize current money from the TextMeshPro text
        if (moneyText != null && int.TryParse(moneyText.text, out int money))
        {
            currentMoney = money;
        }
        else
        {
            Debug.LogError("Money Text is not assigned or invalid!");
        }
    }

    public void ScaleBounce(int repeatCount)
    {
        if (repeatCount <= 0)
        {
            Debug.LogWarning("Repeat count must be greater than 0!");
            return;
        }

        if (moneyText == null)
        {
            Debug.LogError("Money Text is not assigned!");
            return;
        }

        // Reset the object's scale to normal before starting
        transform.localScale = Vector3.one;

        // Create a DOTween sequence for chaining animations
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < repeatCount; i++)
        {
            // Scale up
            sequence.Append(transform.DOScale(Vector3.one * scaleFactor, animationDuration).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    // Decrease the money value
                    if (currentMoney > 0)
                    {
                        currentMoney--;
                        moneyText.text = currentMoney.ToString();
                    }
                }));

            // Scale back to normal
            sequence.Append(transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.InQuad));
        }

        // Play the sequence
        sequence.Play();
    }
}
