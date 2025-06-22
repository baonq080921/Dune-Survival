using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.AI.Navigation;
using UnityEngine;
using System;

public class LevelGenerator : MonoBehaviour
{

    public static LevelGenerator instance;

    [SerializeField] private List<Enemy> enemyList = new List<Enemy>();
    [SerializeField] private List<Transform> levelParts;
    [SerializeField] private NavMeshSurface navMeshSurface;

    public List<Transform> currentLevelParts;
    private List<Transform> generatedLevelParts = new List<Transform>();
    [SerializeField] private Transform lastLevelPart;
    [SerializeField] private SnapPoint nextSnapPoint;
    private SnapPoint defaultSnapPoint;


    [Space]
    [SerializeField] private float generationLevelCoolDown;
    private float coolDownTimer;
    private bool generationOver = true;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        defaultSnapPoint = nextSnapPoint;
    }

    void Update()
    {
        if (generationOver)
            return;
        coolDownTimer -= Time.deltaTime;
        if (coolDownTimer < 0)
        {
            if (currentLevelParts.Count > 0)
            {
                coolDownTimer = generationLevelCoolDown;
                GenerateNextLevelPart();
            }
            else if (generationOver == false)
            {
                FinishGeneration();
            }

        }
    }

    private void FinishGeneration()
    {

        generationOver = true;
        GenerateNextLevelPart();
        navMeshSurface.BuildNavMesh();

        foreach (Enemy enemy in enemyList)
        {
            if (enemy != null)
            {
                enemy.transform.parent = null;
            }
        }

        // call this to create a mission when we finishing generation the map
        MissionManager.instance.StartMission();





    }

    [ContextMenu("Create next level part")]
    private void GenerateNextLevelPart()
    {
        Transform newPart = null;

        if (generationOver)
            newPart = Instantiate(lastLevelPart);
        else
            newPart = Instantiate(chooseRandomPart());

        generatedLevelParts.Add(newPart);


        Enemy[] enemiesInPart = newPart.GetComponentsInChildren<Enemy>(true);
        foreach (Enemy enemy in enemiesInPart)
        {
            if (!enemyList.Contains(enemy))
                enemyList.Add(enemy);
        }

        LevelPart levelPartScript = newPart.GetComponent<LevelPart>();
        levelPartScript.SnapAndAlignPartTo(nextSnapPoint);

        if (levelPartScript.IntersectionDetected())
        {
            InitializeLevelPart();
            return;
        }

        nextSnapPoint = levelPartScript.GetExitPoint();
    }

    [ContextMenu("Restart generation")]
    public void InitializeLevelPart()
    {
        nextSnapPoint = defaultSnapPoint;
        generationOver = false;
        currentLevelParts = new List<Transform>(levelParts);


        DestroyOldLevelPartsAndEnemies();
    }


    private void DestroyOldLevelPartsAndEnemies()
    {



        foreach (Transform t in generatedLevelParts)
        {
            Destroy(t.gameObject);
        }

        generatedLevelParts = new List<Transform>();
    }
    private Transform chooseRandomPart()
    {
        if (currentLevelParts.Count < 0)
            return null;

        Transform choossenPart = currentLevelParts[0];

        currentLevelParts.RemoveAt(0);
        return choossenPart;
    }

    public Enemy GetRandomEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemyList.Count);
        return enemyList[randomIndex];
    }

    public List<Enemy> GetEnemyBoss()
    {
        List<Enemy> bossList = new List<Enemy>();
        foreach (var enemy in enemyList)
        {
            if (enemy.enemyType == EnemyType.Boss)
            {
                bossList.Add(enemy);
            }
        }
        return bossList;
    }

    public List<Enemy> GetEnemies() => enemyList;
}
