using UnityEngine;

public class MoveState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    private Vector3 destination;
    private float actionTimer;
    private float timeBeforeSpeedUp = 2f;
    private bool isSpeedUpActivated;
    public MoveState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }
    public override void Enter()
    {
        base.Enter();

        SpeedReset();

        enemy.agent.speed = enemy.walkSpeed;
        enemy.agent.isStopped = false;

        destination = enemy.GetPatrolDestination();
        enemy.agent.SetDestination(destination);
        actionTimer = enemy.actionCoolDown;

    }

   

    public override void Update()
    {
        base.Update();
        enemy.FaceTarget(GetNextPathPoint());
        actionTimer -= Time.deltaTime;

        if (enemy.inBattleMode)
        {
            if (ShouldSpeedUp())
            {
                SpeedUp();
            }
            Vector3 playerPos = enemy.player.position;
            enemy.agent.SetDestination(playerPos);
            //if enemy in battle mode
            if (actionTimer < 0)
            {
                PerformRandomAction();
            }
            // Logic of enemy AttackState change:
            else if (enemy.PlayerInAttackRange())
                stateMachine.ChangeState(enemy.attackState);
        }
        else
        {
            if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance + .05f)
                stateMachine.ChangeState(enemy.idleState);

        }

    }

    // Change the animation to walk or run

     private void SpeedReset()
    {
        isSpeedUpActivated = false;

        enemy.anim.SetFloat("MoveAnimIndex", 0);
        enemy.agent.speed = enemy.walkSpeed;
    }
    private void SpeedUp()
    {
        isSpeedUpActivated = true;
        enemy.agent.speed = enemy.chaseSpeed;
        enemy.anim.SetFloat("MoveAnimIndex", 1);
    }

    private bool ShouldSpeedUp()
    {
        if (isSpeedUpActivated) return false;
        if (Time.time > enemy.attackState.lastTimeAttack + timeBeforeSpeedUp)
        {
            return true;
        }
        return false;
    }


    //  Perform a random action based on the enemy's abilities cooldown
    private void PerformRandomAction()
    {
        actionTimer = enemy.actionCoolDown;

        if (Random.Range(0, 2) == 0)
        {
            TryAbility();
        }
        else
        {
            if (enemy.CanDoJumpAttack())
                stateMachine.ChangeState(enemy.jump_AttackState);
            else if (enemy.bossWeaponType == BossWeaponType.Hummer)
                TryAbility();

        }
    }

    private void TryAbility()
    {
        if (enemy.CanDoAbility())
        {
            stateMachine.ChangeState(enemy.abilityState);
        }
    }
}
