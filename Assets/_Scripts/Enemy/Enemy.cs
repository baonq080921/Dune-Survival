using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum EnemyType{Melee, Range, Boss,Random};

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{

    public LayerMask whatisAlly;
    public LayerMask WhatIsPlayer;
    // public int healthPoints = 20;

    public EnemyType enemyType;


    
    [Header("Idle data")]
    public float idleTime;
    public float aggresionRange;

    [Header("Move data")]
    public float walkSpeed;
    public float chaseSpeed;
    public float turnSpeed;
    private bool manualMovement;
    private bool manualRotation;

    protected bool isMeleeAttackReady;
    

    [SerializeField] private Transform[] patrolPoints;
    private Vector3[] patrolPointsPosition;
    private int currentPatrolIndex;



    public bool inBattleMode { get; private set; }
    public bool isDead;

    public Transform player { get; private set; }
    public Animator anim { get; private set; }
    public EnemyStateMachine stateMachine { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public Enemy_Visuals visuals { get; private set; }
    public Ragdoll enemy_Ragdoll { get; private set; }

    public Enemy_Health enemy_Health { get; private set; }
    

    public EnemyDrop_Controller enemyDrop_Controller { get; private set; }


    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();
        visuals = GetComponent<Enemy_Visuals>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        enemy_Health = GetComponent<Enemy_Health>();
        enemyDrop_Controller = GetComponent<EnemyDrop_Controller>();
    }

    protected virtual void Start()
    {
        InitializePatrolPoints();
    }



    // protected virtual void Update()
    // {
    //     if (ShouldEnterBattleMode())
    //         EnterBattleMode();
    // }


    protected virtual void Update()
    {
        bool inAggresionRange = Vector3.Distance(transform.position, player.position) < aggresionRange;
        if (isDead) return;
        if (inAggresionRange)
        {
            if (ShouldEnterBattleMode())
                EnterBattleMode();
        }
        else
        {
            if (inBattleMode)
                ExitBattleMode();
        }
    }
    protected bool ShouldEnterBattleMode()
    {
        bool inAggresionRange = Vector3.Distance(transform.position, player.position) < aggresionRange;

        if (inAggresionRange && !inBattleMode)
        {
            EnterBattleMode();
            return true;
        }

        return false;
    }

    
    public virtual void EnterBattleMode()
    {
        inBattleMode = true;
    }

    public virtual void ExitBattleMode()
{
    inBattleMode = false;
}

    public virtual void GetHit(int damage)
    {
        enemy_Health.ReduceHealth(damage);
        
        if (enemyType == EnemyType.Range)//change the range of the Range enemy from move state to shoot state range
        {
            aggresionRange = 5f;
        }

        if (enemy_Health.ShouldDie())
        {
            Die();
        }

        EnterBattleMode();
    }


    public virtual void Die()
    {

        enemyDrop_Controller.DropItems();
        MissionObject_HuntTarget huntTarget = GetComponent<MissionObject_HuntTarget>();
        huntTarget?.InvokeOnTargetKilled();

    }



public void MeleeAttackCheck(Transform[]damagePoints, int attackDamage, float attackCheckRadius, GameObject fx )
    {
        if (isMeleeAttackReady == false)
            return;

        foreach (Transform attackPoint in damagePoints)
        {
            Collider[] detectedHits =
                Physics.OverlapSphere(attackPoint.position, attackCheckRadius,WhatIsPlayer);


            for (int i = 0; i < detectedHits.Length; i++)
            {
                IDamagable damagable = detectedHits[i].GetComponent<IDamagable>();

                if (damagable != null)
                {
                    damagable.TakeDamage(attackDamage);
                    isMeleeAttackReady = false;
                    GameObject newattackFx = ObjectPool.instance.GetObject(fx, attackPoint);
                    ObjectPool.instance.ReturnObject(newattackFx, 1);
                    return;
                }
            }

        }
        
    }


public void EnableMeleeAttackCheck(bool enable) => isMeleeAttackReady = enable;


public virtual void BulletImpact(Vector3 force, Vector3 hitPoint, Rigidbody rb)
{
    if (enemy_Health.ShouldDie())
        StartCoroutine(DeathImpactCourutine(force, hitPoint, rb));
}
private IEnumerator DeathImpactCourutine(Vector3 force, Vector3 hitPoint, Rigidbody rb)
{
    yield return new WaitForSeconds(.1f);

    if(rb != null)
        rb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
}

public void FaceTarget(Vector3 target, float turnSpeed = 0)
{
    Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

    Vector3 currentEulerAngels = transform.rotation.eulerAngles;
    if (turnSpeed != 0)
    {
        turnSpeed = this.turnSpeed;
        // Debug.Log(turnSpeed);
    }

    float yRotation =
            Mathf.LerpAngle(currentEulerAngels.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);

    transform.rotation = Quaternion.Euler(currentEulerAngels.x, yRotation, currentEulerAngels.z);
}




// public void MakEnemyVip()
// {
//     int additionalHealth = Mathf.RoundToInt(enemy_Health.currentHealth * 1.5f);
//     enemy_Health.currentHealth += additionalHealth;

//     // Scale the mesh child, not just the root
//     if (skinnedMeshRenderer != null)
//     {
//         skinnedMeshRenderer.transform.localScale *= 1.5f; // or any factor you want
//     }

//     // Optionally, also scale the root if you want to affect colliders/agent
//     transform.localScale *= 1.5f;

//     Material newMat = new Material(skinnedMeshRenderer.material);
//     newMat.mainTexture = colorTextures;
//     skinnedMeshRenderer.material = newMat;
// }



    #region Animation events
    public void ActivateManualMovement(bool manualMovement) => this.manualMovement = manualMovement;
    public bool ManualMovementActive() => manualMovement;

    public void ActivateManualRotation(bool manualRotation) => this.manualRotation = manualRotation;
    public bool ManualRotationActive() => manualRotation;
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();



    public virtual void AbilityTrigger()
    {
        stateMachine.currentState.AbilityTrigger();
    }

    #endregion

    #region Patrol logic
    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPointsPosition[currentPatrolIndex];
        // Debug.Log($"Patrol index: {currentPatrolIndex}, destination: {destination}");
        currentPatrolIndex++;

        if (currentPatrolIndex >= patrolPoints.Length)
            currentPatrolIndex = 0;

        return destination;
    }
    private void InitializePatrolPoints()
    {
        patrolPointsPosition = new Vector3[patrolPoints.Length];

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPointsPosition[i] = patrolPoints[i].position;
            patrolPoints[i].gameObject.SetActive(false);
        }
    }

    #endregion


    protected virtual void OnDrawGizmos()
    {
        Gizmos.color= Color.blue;
        Gizmos.DrawWireSphere(transform.position, aggresionRange);
    }
}
