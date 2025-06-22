using UnityEngine;

public abstract class Mission : ScriptableObject
{
    public string missionName;
    public string missionDescription;
    public abstract void StartMission();

    public abstract bool MissionCompleted();

    public virtual void UpdatedMission()
    {
         Debug.Log("UpdatedMission called, UI.instance: " + (UI.instance != null) + ", inGameUI: " + (UI.instance?.inGameUI != null));
        
    }


}
