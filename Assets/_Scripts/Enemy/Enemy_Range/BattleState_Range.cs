using UnityEngine;

public class BattleState_Range : EnemyState
{


    public float lastTimeShot = -10f;
    private int bulletShot = 0;
    private float turnSpeed = 3f;

    private Enemy_Range enemy;
    public BattleState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {

        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.FaceTarget(enemy.player.position, turnSpeed);
        // Stop movement
        var agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
       if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
             agent.isStopped = true;

    }

    public override void Exit()
    {
        base.Exit();

        enemy.ExitBattleMode();
         // Resume movement
        var agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
         if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
                agent.isStopped = false;
    }

    public override void Update()
    {

        if (enemy.isDead)
            return;
            
        base.Update();

        enemy.FaceTarget(enemy.player.position,turnSpeed);
        if (WeaponOutOfAmmo())
        {

            if (WeaponOnCooldown())
            {
                AttemptToShoot();
            }
            return;
        }

        if (CanShot())
        {
            Shoot();
        }

        //check if the player is out of range then change state to move
        if (enemy.IsPlayerOutOfRange())
        {
            // Debug.Log("Player out of range");
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
        
    }

    private void AttemptToShoot() => bulletShot = 0;

    private bool CanShot() => Time.time > lastTimeShot + 1 / enemy.fireRate;

    private bool WeaponOutOfAmmo() => bulletShot >= enemy.bulletToShoot;

    private bool WeaponOnCooldown() => Time.time >  lastTimeShot + enemy.weaponCooldown;

    private void Shoot()
    {
        enemy.FireSingleBullet();
        lastTimeShot = Time.time;
        bulletShot++;
    }
}
