using UnityEngine;

public class IdleState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    public IdleState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();
        if (enemy.PlayerInAttackRange() && enemy.inBattleMode)
        {
           // Debug.Log("Hello world");
            stateMachine.ChangeState(enemy.attackState);
        }
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
