using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MissionSelectedButton : UI_Buttons
{

    [SerializeField] private Mission myMission;
    private UI_MissionSelection missionUI;
    private TextMeshProUGUI textMeshPro ;


  
    void OnValidate()
    {
        gameObject.name = "Button - select Mission: " + myMission.name;
    }
    public override void Start()
    {
        base.Start();

        missionUI = GetComponentInParent<UI_MissionSelection>();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.text = myMission.name;


    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        Debug.Log(myMission.missionDescription);
        missionUI.UpdateMissionTextDescription(myMission.missionDescription);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        missionUI.UpdateMissionTextDescription("Choose a mission");
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        MissionManager.instance.SelectMission(myMission);
    }
}
