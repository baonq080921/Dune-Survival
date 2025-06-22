using UnityEngine;


[CreateAssetMenu(fileName ="new Timer Mission", menuName ="Missions/Timer missions")]
public class Mission_TImer : Mission
{


    public float time;
    private float currentTime;
    private MissionEnd_Trigger missionEnd_Trigger;
    public override void StartMission()
    {
        currentTime = time;
        missionEnd_Trigger = FindObjectOfType<MissionEnd_Trigger>();

    }

    public override void UpdatedMission()
    {
        base.UpdatedMission();
        currentTime -= Time.deltaTime;
        if (UI.instance == null || UI.instance.inGameUI == null)
            return;

        // Chỉ update UI khi UI đã sẵn sàng
            if (UI.instance == null || UI.instance.inGameUI == null)
                return;

        if (currentTime < 0)
        {
            string gameOverText = "Time up!! Game over!!!";
            UI.instance.inGameUI.UpdateUIMissionInfo(gameOverText);
        }

        string timeText ="Time Left: "+ System.TimeSpan.FromSeconds(Mathf.Max(0, currentTime)).ToString("mm':'ss");
        string missionText = "Get to the finish line one time";
        string distanceToFinishText = missionEnd_Trigger != null
                         ? missionEnd_Trigger.DistanceToFinishLine().ToString()
                         : "N/A";
        if (missionEnd_Trigger.DistanceToFinishLine() < 10f)
        {
            distanceToFinishText = "Come on you almost there keep going forward !!!!";
        }
        UI.instance.inGameUI.UpdateUIMissionInfo(missionText,timeText,"You are away from the finish line "+distanceToFinishText);
    }
    public override bool MissionCompleted()
    {
        
        return currentTime > 0;
    }

}
