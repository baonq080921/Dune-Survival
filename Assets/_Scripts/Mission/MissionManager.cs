using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    //public Mission_LastDefence mission_LastDefence;

    public Mission currentMission;

    void Awake()
    {
        instance = this;
    }
    
    void Update()
    {
        currentMission?.UpdatedMission();



    }

    public void SelectMission(Mission newMission)
    {
        currentMission = newMission;
    }
    

    public void StartMission() => currentMission.StartMission();

    public bool MissionCompleted() => currentMission.MissionCompleted();


}
