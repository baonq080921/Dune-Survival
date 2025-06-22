using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;


[CreateAssetMenu(menuName = "Missions/Last Defence", fileName = "New Last Defence Mission")]

public class Mission_LastDefence : Mission
{

    public bool defenceBegun = false;
    public bool bossSpawned = false;
    [Header("Cool Down and duration")]
    public float defenceDuration = 120f;
    private float defenceTimer;
    public float waveCoolDown = 15f;
    public float bossTimeRespawn;
    private float waveTimer;

    [Header("Respawn details")]
    public int amountOfRespawnPoints = 3; // direction of the enemy respawn from

    public List<Transform> respawnPoints;

    private Vector3 defencePoint;
    [Space]
    public int enemiesPerWave;
    public GameObject[] possibleEnemies;
    public GameObject[] possibleBoss;




    void OnEnable()
    {
        defenceBegun = false;
        bossSpawned = false;

    }

    public override void StartMission()
    {
        defencePoint = FindObjectOfType<MissionEnd_Trigger>().transform.position;

        respawnPoints = new List<Transform>(ClosetPoints(amountOfRespawnPoints));

        UI.instance.inGameUI.UpdateUIMissionInfo(missionDescription);

    }

    public override void UpdatedMission()
    {
        base.UpdatedMission();
        // if will start whenever we reach the final destination
        if (defenceBegun == false)
            return;


        if(defenceTimer >0)
            defenceTimer -= Time.deltaTime;
        waveTimer -= Time.deltaTime;
        string timeText = "Time Left: "+System.TimeSpan.FromSeconds(Mathf.Max(0, defenceTimer)).ToString("mm':'ss");
        string missionText = "Defend yourself till the plane is ready to take off. ";

        UI.instance.inGameUI.UpdateUIMissionInfo(missionText, timeText);

        // Create boss for the defenceTime < time that we define .
        if (!bossSpawned && defenceTimer < bossTimeRespawn)
        {
            CreateNewBoss(2);
            bossSpawned = true;
        }
        if (waveTimer < 0)
        {
            CreateNewEnemies(enemiesPerWave);
            waveTimer = waveCoolDown;
        }
    }
    public override bool MissionCompleted()
    {
        if (defenceBegun == false)
        {
            StartDefenceEvent();
            return false;
        }
        string text = "Go Go Go !! THe PLane is ready";
        UI.instance.inGameUI.UpdateUIMissionInfo(text);

        DestroyAllEnemies();
        return defenceTimer < 0; // return if we defencet succes through ammout of time
    }


    private void StartDefenceEvent()
    {
        waveTimer = .5f;
        defenceTimer = defenceDuration;
        defenceBegun = true;
    }

    private void CreateNewEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomEnemyIndex = Random.Range(0, possibleEnemies.Length);
            int randomRespawnIndex = Random.Range(0, respawnPoints.Count);
            Transform randomRespawnPoint = respawnPoints[randomRespawnIndex];
            GameObject randomEnemy = possibleEnemies[randomEnemyIndex];
            randomEnemy.GetComponent<Enemy>().aggresionRange = 100f;
            ObjectPool.instance.GetObject(randomEnemy, randomRespawnPoint);
        }
    }

    private void CreateNewBoss(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomEnemyIndex = Random.Range(0, possibleBoss.Length);
            int randomRespawnIndex = Random.Range(0, respawnPoints.Count);
            Transform randomRespawnPoint = respawnPoints[randomRespawnIndex];
            GameObject randomEnemy = possibleBoss[randomEnemyIndex];
            randomEnemy.GetComponent<Enemy>().aggresionRange = 100f;
            ObjectPool.instance.GetObject(randomEnemy, randomRespawnPoint);
        }
    }

    private List<Transform> ClosetPoints(int amount)
    {
        List<Transform> closetPoints = new List<Transform>();
        List<MissionObject_EnemyRespawnPoint> allPoints =
         new List<MissionObject_EnemyRespawnPoint>(FindObjectsOfType<MissionObject_EnemyRespawnPoint>());


        while (closetPoints.Count < amount && allPoints.Count > 0)
        {
            float shortestDistance = float.MaxValue;
            MissionObject_EnemyRespawnPoint closetPoint = null;

            foreach (var point in allPoints)
            {
                float distance = Vector3.Distance(point.transform.position, defencePoint);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closetPoint = point;
                }
            }

            if (closetPoint != null)
            {
                closetPoints.Add(closetPoint.transform);
                allPoints.Remove(closetPoint);
            }
        }
        return closetPoints;

    }

    public void DestroyAllEnemies()
    {
        //Destroy all the enenimes in the screen;
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            if (enemy != null)
                Destroy(enemy.gameObject);
        }
    }
   
}
