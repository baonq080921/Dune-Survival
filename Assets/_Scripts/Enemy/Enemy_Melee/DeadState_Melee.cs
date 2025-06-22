using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private Ragdoll ragdoll;

    private bool interactionDisabled;

    public DeadState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
        ragdoll = enemy.GetComponent<Ragdoll>();
    }

    public override void Enter()
{
    base.Enter();

    interactionDisabled = false;

    enemy.anim.enabled = false;

    // Only stop agent if enabled and on NavMesh
    if (enemy.agent.enabled && enemy.agent.isOnNavMesh)
        enemy.agent.isStopped = true;

    enemy.agent.enabled = false;

    ragdoll.RagdollActive(true);
    enemy.isDead = true;


    stateTimer = 1.5f;
}
    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // uncommnet if you want to disale interaction 
        //DisableInteractionIfShould();
    }

    private void DisableInteractionIfShould()
    {
        if (stateTimer < 0 && interactionDisabled == false)
        {
            interactionDisabled = true;
            ragdoll.RagdollActive(false);
            ragdoll.CollidersActive(false);
        }
    }
}
