using UnityEngine;
using TMPro;

public class UICreditsRoll : MonoBehaviour
{
    public RectTransform creditsText; // Assign in Inspector
    public float speed = 5f;         // Pixels per second

    private Vector2 startPos;
    private float endY;

    void OnEnable()
    {
        startPos = creditsText.anchoredPosition;
        // Calculate endY so text scrolls completely out of view
        endY = startPos.y + creditsText.rect.height + ((RectTransform)transform).rect.height;
    }

    void Update()
    {
        creditsText.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        // Stop when credits have fully scrolled out (optional)
        if (creditsText.anchoredPosition.y >= endY)
        {
            // Optionally, disable or reset
            // gameObject.SetActive(false);
        }
    }

    public void ResetCredits()
    {
        creditsText.anchoredPosition = startPos;
    }
}