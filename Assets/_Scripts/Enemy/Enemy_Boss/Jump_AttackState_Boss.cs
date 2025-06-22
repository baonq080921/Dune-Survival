using UnityEditor.Scripting;
using UnityEngine;

public class Jump_AttackState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    private Vector3 lastPlayerPos;
    private float jumpAttackMovementSpeed;
    public Jump_AttackState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter()
    {
        base.Enter();
        lastPlayerPos = enemy.player.position; // take the last pos of the player 
        //enemy.agent.isStopped = true;
        // enemy.agent.velocity = Vector3.zero;
        float distanceToPlayer = Vector3.Distance(lastPlayerPos, enemy.transform.position);
        enemy.enemyBoss_Visual.PlaceLandingZone(lastPlayerPos);
        enemy.enemyBoss_Visual.EnableWeaponTrail(true);
        jumpAttackMovementSpeed = distanceToPlayer / enemy.travelTimeToTarget;

        enemy.FaceTarget(lastPlayerPos, enemy.turnSpeed);


        if (enemy.bossWeaponType == BossWeaponType.Hummer)
        {
            enemy.agent.isStopped = false;
            enemy.agent.speed = enemy.chaseSpeed;
            enemy.agent.SetDestination(lastPlayerPos);
        }
    }
    public override void Update()
    {
        base.Update();

        Vector3 myPos = enemy.transform.position;
        //
        enemy.agent.enabled = !enemy.ManualMovementActive();
        if (enemy.ManualMovementActive())
        {
            enemy.transform.position = Vector3.MoveTowards(myPos, lastPlayerPos, jumpAttackMovementSpeed * Time.deltaTime);
        }
        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.SetJumpAttackOnCoolDown();
        enemy.enemyBoss_Visual.EnableWeaponTrail(false);

    }
}
