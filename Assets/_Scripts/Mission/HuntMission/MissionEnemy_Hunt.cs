using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Missions/Hunt Missions", fileName = "New Hunt Mission")]

public class MissionEnemy_Hunt : Mission
{

    public int ammoutToKill = 12;
    public EnemyType enemyType;
    private int killToGo;
    public override void StartMission()
    {

        killToGo = ammoutToKill;
        UpdateMissionUI();

        MissionObject_HuntTarget.OnTargetKilled += EliminateTarget;



        List<Enemy> validEneimes = new List<Enemy>();

        foreach (Enemy enemy in LevelGenerator.instance.GetEnemies())
        {
            if (enemy.enemyType == enemyType)
                validEneimes.Add(enemy);
        }

        for (int i = 0; i < ammoutToKill; i++)
        {
            // Remove any destroyed enemies from the list
            validEneimes.RemoveAll(e => e == null);

            if (validEneimes.Count <= 0)
                break;

            int randomIndex = Random.Range(0, validEneimes.Count);

            if (validEneimes[randomIndex] != null)
            {
                validEneimes[randomIndex].AddComponent<MissionObject_HuntTarget>();
                validEneimes.RemoveAt(randomIndex);
            }
        }


    }
    public override bool MissionCompleted()
    {
        return killToGo <= 0;
    }

    private void EliminateTarget()
    {
        killToGo--;
        UpdateMissionUI();
        if (killToGo <= 0)
            UI.instance.inGameUI.UpdateUIMissionInfo("Get to the plane right now!!You defend the plane success.");
        MissionObject_HuntTarget.OnTargetKilled -= EliminateTarget;
    }

    private void UpdateMissionUI()
    {
        string missionText = "You need to hunt down the enemy in " + ammoutToKill
                                + " enemy right now find the target or you will not comehome one have a cure ";
        string missionDetails = "Target left: " + killToGo;
        UI.instance.inGameUI.UpdateUIMissionInfo(missionText, missionDetails);
    }

    
}
