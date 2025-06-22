using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public enum BossWeaponType { FlameThrower, Hummer}
public class Enemy_Boss : Enemy
{

    public EnemyBoss_Visual enemyBoss_Visual;


    [Header("Boss details")]

    public BossWeaponType bossWeaponType;

    public float actionCoolDown = 10f;
    public float attackRange;
    public int attackDamage;

    [Header("Ability")]
    public float minAbilityDistance;
    public float abilityCoolDown;
    private float lastTimeUsedAbiliy;

    [Header("FlameThrower")]
    public int flameDamage;
    public float flameDamageCoolDown;
    public float flameThrowDuration;
    public ParticleSystem flameThrow;
    public bool flameThrowActive { get; private set; }

    [Header("Hummer")]
    public GameObject hummeFxPrefab;



    [Header("Jump Attack")]
    public float travelTimeToTarget = 1f;
    public float timeJumpAttackCoolDown = 10f;
    [SerializeField] private Transform impactPoint;
    private float lastTimeJumped;
    public float minJumpedDistance;
    [SerializeField] private int jumpDamaged;
    [Space]
    public float impactRadius = 2.5f;
    public float impactPower = 5f;
    [SerializeField] private float upforceMultiplier;

    [Header("Attack")]
    [SerializeField] private Transform[] damagePoints;
    [SerializeField] private float attackCheckRadius;
    [SerializeField] private GameObject meleeAttackFx;
    [SerializeField] private int attackDamge;
    private bool isAttackReady;
   
    [Space]

    [Header("Camera Transiton")]
    private bool hasFocus = false;
    private float focusDuration = 2f;

    [Space]

    [SerializeField] LayerMask whatToIgnore;
    public IdleState_Boss idleState { get; private set; }
    public MoveState_Boss moveState { get; private set; }
    public AttackState_Boss attackState { get; private set; }
    public Jump_AttackState_Boss jump_AttackState { get; private set; }
    public AbilityState_Boss abilityState { get; private set; }
    public DeadState_Boss deadState { get; private set; }
    public Enemy_Health health { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        enemyBoss_Visual = GetComponent<EnemyBoss_Visual>();
        health = GetComponent<Enemy_Health>();
        idleState = new IdleState_Boss(this, stateMachine, "Idle");
        moveState = new MoveState_Boss(this, stateMachine, "Move");
        attackState = new AttackState_Boss(this, stateMachine, "Attack");
        jump_AttackState = new Jump_AttackState_Boss(this, stateMachine, "jumpAttack");
        abilityState = new AbilityState_Boss(this, stateMachine, "Ability");
        deadState = new DeadState_Boss(this, stateMachine, "Idle");
    }


    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);

    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        //Checking if we 
        EnterTheBossStage();
        if (ShouldEnterBattleMode())
        {
            EnterBattleMode();
        }
        MeleeAttackCheck(damagePoints, attackDamage, attackCheckRadius, meleeAttackFx);

        // AttackCheck(damagePoints, attackRadius, meleeAttackFx);
    }


    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackRange;

    public bool CanDoJumpAttack()
    {
        // Debug.Log("Is PLayer is  in sight clear" + IsPlayerSightClear());
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < minJumpedDistance) return false;
        if (Time.time > lastTimeJumped + timeJumpAttackCoolDown && IsPlayerSightClear())
        {
            // Debug.Log("Why u not jumping");

            return true;
        }
        return false;
    }    




    public override void Die()
    {
        base.Die();
        if (stateMachine.currentState != deadState)
            stateMachine.ChangeState(deadState);
    }


    public bool CanDoAbility()
    {

        bool playerWithinDistance = Vector3.Distance(transform.position, player.position) < minAbilityDistance;

        if (playerWithinDistance == false)
        {
            return false;
        }


        if (Time.time > lastTimeUsedAbiliy + abilityCoolDown && playerWithinDistance)
        {
            return true;
        }

        return false;
    }


    public void SetAbilityOnCoolDown() => lastTimeUsedAbiliy = Time.time;
    public void SetJumpAttackOnCoolDown() => lastTimeJumped = Time.time;
    public void ActiveFlameThrower(bool active)
    {
        flameThrowActive = active;
        // Debug.Log("Flame throwe activated " + active);
        if (!active)
        {
            flameThrow.Stop();
            anim.SetTrigger("StopFlameThrower");
            Debug.Log("Flame Stopped");
            return;
        }


        var mainModule = flameThrow.main;
        var subMainModule = flameThrow.transform.GetComponentInChildren<ParticleSystem>().main;
        mainModule.duration = flameThrowDuration;
        subMainModule.duration = flameThrowDuration;
        flameThrow.Clear();
        flameThrow.Play();
    }


    public void ActiveHummer()
    {
        GameObject newActivation = ObjectPool.instance.GetObject(hummeFxPrefab, impactPoint);

        ObjectPool.instance.ReturnObject(newActivation, 1);
    }
    public bool IsPlayerSightClear()
    {
        Vector3 myPos = transform.position + new Vector3(0, 1.5f, 0);
        Vector3 playerPos = player.position + Vector3.up;
        Vector3 directionToPlayer = (playerPos - myPos).normalized;
        // Debug.Log("player to enemies:  " + directionToPlayer);

        if (Physics.Raycast(myPos, directionToPlayer, out RaycastHit hit, 100, ~whatToIgnore))
        {
            if (hit.transform == player || hit.transform.parent == player)
                return true;
        }
        return false;
    }


    public void JumpImpact()
    {

        Transform impactPoint = this.impactPoint;
        if (impactPoint == null)
        {
            impactPoint = transform;
        }

        MassDamage(impactPoint.position, impactRadius, jumpDamaged);
        
    }

    private void MassDamage(Vector3 impactP, float impactR, int damage )
    {

        HashSet<GameObject> uniqueEntities = new HashSet<GameObject>();
        Collider[] colliders = Physics.OverlapSphere(impactP, impactR, ~whatisAlly);

        foreach (Collider hit in colliders)
        {

            IDamagable damagable = hit.GetComponent<IDamagable>();
            if (damagable != null)
            {
                GameObject rootEntity = hit.transform.root.gameObject;

                if (uniqueEntities.Add(rootEntity) == false)
                    continue;

                damagable?.TakeDamage(damage);
            }
            ApplyPhysicalForceTo(hit);
        }
    }

    private void ApplyPhysicalForceTo(Collider hit)
    {
        Rigidbody rb = hit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddExplosionForce(impactPower, transform.position, impactRadius, upforceMultiplier, ForceMode.Impulse);
        }
    }

    private void EnterTheBossStage()
    {
        bool inAggersionRange = Vector3.Distance(transform.position, player.position) < aggresionRange;
        if (!hasFocus && inAggersionRange)
        {
            hasFocus = true;
            StartCoroutine(FocusOnTheBoss());
        }
    }

    IEnumerator FocusOnTheBoss()
    {
        CameraManager.instance.virtualCamera.Priority = 10;
        CameraManager.instance.virtualCameraBoss.Priority = 20 ;

        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(focusDuration);

        CameraManager.instance.virtualCamera.Priority =20;
        CameraManager.instance.virtualCameraBoss.Priority = 10 ;


        Time.timeScale = 1f;

    }


    public override void EnterBattleMode()
    {
        // if (inBattleMode) return;
        base.EnterBattleMode();

        // stateMachine.ChangeState(moveState);

    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        // attack Range gizmos
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (player != null)
        {
            // draw a line from enem to player for debug 
            Vector3 myPos = transform.position + new Vector3(0, 1.5f, 0);
            Vector3 playerPos = player.position + Vector3.up;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(myPos, playerPos);
        }

        // Debug gizmos for jumpdistance
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minJumpedDistance);

        //Debug gizmos for impactRadius


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, impactRadius);

        // Debug gizmos for minAbility
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, minAbilityDistance);


        if (damagePoints.Length > 0)
        {
            foreach (Transform damagePoint in damagePoints)
            {
                Gizmos.DrawWireSphere(damagePoint.position, attackCheckRadius);
            }
        }

    }
}
