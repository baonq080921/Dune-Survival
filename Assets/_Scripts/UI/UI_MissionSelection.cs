using TMPro;
using UnityEngine;

public class UI_MissionSelection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionText;


    public void UpdateMissionTextDescription(string text)
    {
        missionText.text = text;
    }
}
