using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Key Missions", fileName = "New Key Mission")]
public class Mission_KeyFind : Mission
{

    [SerializeField] private GameObject key;
    [SerializeField] private GameObject bossKey;
    private List<Enemy> enemiesWithKey = new List<Enemy>(); // list of enemy that how the key
    public int numberOfKeyEnemies = 3;  // numbers of key public int bossKey = 2;
    public int numberOfKeyEnemiesBoss = 2;
    public int totalKey;
    public bool keyFound;
    private int keyCollectedCount;
    //Text Description and Mission Details
    public override void StartMission()
    {
        // From the start of the mission:

        totalKey = numberOfKeyEnemies + numberOfKeyEnemiesBoss;
        UI.instance.inGameUI.UpdateUIMissionInfo("Find a key holder they among the enemy.Retrive all the key.", "There are " + totalKey + " key in total.Find it all!!!!");
        MissionObject_Key.OnKeyPickedUp -= PickUpKey; // Prevent double subscription
        MissionObject_Key.OnKeyPickedUp += PickUpKey;
        keyFound = false;
        keyCollectedCount = 0;
        //find random enemy
        // give key to random enemies;
        for (int i = 0; i < numberOfKeyEnemies; i++)
        {
            Enemy enemy = LevelGenerator.instance.GetRandomEnemy();

            if (enemy != null)
            {
                enemy.GetComponent<EnemyDrop_Controller>()?.GiveKey(key);
                enemiesWithKey.Add(enemy);

            }
        }




        //This is special key for boss:
        List<Enemy> enemyBossList = LevelGenerator.instance.GetEnemyBoss();
        Debug.Log(enemyBossList.Count);
        foreach (Enemy enemy in enemyBossList)
        {
            enemy.GetComponent<EnemyDrop_Controller>()?.GiveKey(bossKey);
        }








    }
    public override bool MissionCompleted()
    {
        return keyFound;
    }

    private void PickUpKey()
    {
        if (keyFound) return;

        //Debug.Log("I Picked up the key. Total Key " + keyCollectedCount);
        UI.instance.inGameUI.UpdateUIMissionInfo(missionDescription, "I Picked up the key. Key Left " + totalKey);
        totalKey --;

        if (totalKey <= 0)
            FoundAllTheKey();
    }

    private void FoundAllTheKey()
    {
        keyFound = true;
        MissionObject_Key.OnKeyPickedUp -= PickUpKey;
    }
    

    
}
