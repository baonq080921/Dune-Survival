using UnityEngine;

public class AbilityState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    public AbilityState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {

        enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.flameThrowDuration;
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;
        enemy.FaceTarget(enemy.player.position, enemy.turnSpeed);
        enemy.enemyBoss_Visual.EnableWeaponTrail(false);


    }


    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(enemy.player.position, enemy.turnSpeed);
        // Debug.Log(enemy.flameThrowActive);
        // Debug.Log("PLayer position"+enemy.player.position);
        if (ShouldDisableFlameThrower())
        {
            DisableFlameThrower();
        }
        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.moveState);
        }


    }

    private bool ShouldDisableFlameThrower() => stateTimer < 0;


    public void DisableFlameThrower()
    {
        if (enemy.bossWeaponType != BossWeaponType.FlameThrower)
            return;

        if (enemy.flameThrowActive == false)
                return;

        enemy.ActiveFlameThrower(false);

    }


    public override void AbilityTrigger()
    {
        base.AbilityTrigger();
        if (enemy.bossWeaponType == BossWeaponType.FlameThrower)
        {
            enemy.ActiveFlameThrower(true);
            enemy.enemyBoss_Visual.DischargeBatteries();
            enemy.enemyBoss_Visual.EnableWeaponTrail(false);
        }
        if (enemy.bossWeaponType == BossWeaponType.Hummer)
        {
            enemy.ActiveHummer();
            
        }
       
    }
    public override void Exit()
    {
        base.Exit();
        enemy.SetAbilityOnCoolDown();

        enemy.enemyBoss_Visual.ResetBatteries();
        enemy.enemyBoss_Visual.EnableWeaponTrail(false);

    }
}
