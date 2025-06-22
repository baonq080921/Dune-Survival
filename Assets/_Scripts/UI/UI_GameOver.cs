using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameoverText;

    public void ShowGameOverMessage(string message)
    {
        gameoverText.text = message;
    }
}
