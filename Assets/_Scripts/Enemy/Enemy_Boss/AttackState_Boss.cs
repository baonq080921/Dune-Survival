using UnityEngine;

public class AttackState_Boss : EnemyState
{

    public float lastTimeAttack { get; private set; }
    private Enemy_Boss enemy;
    private Vector3 attackDirection;
    private const float MAX_ATTACK_DISTANCE = 50f;

    public AttackState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetFloat("AttackIndex", Random.Range(0, 2));// Random attack move from 0 to 1
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;
        stateTimer = 1f;
        enemy.enemyBoss_Visual.EnableWeaponTrail(true);


    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            enemy.FaceTarget(enemy.player.position,20f);
        }
        if (triggerCalled)
        {
            if (enemy.PlayerInAttackRange())
            {
                // Debug.Log("Hello truererkejrkejr" + enemy.inBattleMode);

                stateMachine.ChangeState(enemy.idleState);
            }
            else
            {
                stateMachine.ChangeState(enemy.moveState);
            }
        }
    }


    public override void Exit()
    {
        base.Exit();
        lastTimeAttack = Time.time;
        enemy.enemyBoss_Visual.EnableWeaponTrail(false);

    }
    
}
