using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AnimationEvents : MonoBehaviour
{
    private Enemy enemy;
    private Enemy_Boss enemyBoss;
    private Enemy_Melee enemyMelee;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        enemyMelee = GetComponentInParent<Enemy_Melee>();
        enemyBoss = GetComponentInParent<Enemy_Boss>();
    }

    public void AnimationTrigger() => enemy.AnimationTrigger();

    public void StartManualMovement() => enemy.ActivateManualMovement(true);
    public void StopManualMovement() => enemy.ActivateManualMovement(false);

    public void StartManualRotation() => enemy.ActivateManualRotation(true);
    public void StopManualRotation() => enemy.ActivateManualRotation(false);

    public void AbilityEvent() => enemy.AbilityTrigger();

    public void BossJumpImpact()
    {
        enemyBoss?.JumpImpact();
    }



    public void BeginMeleeAttackCheck()
    {
        enemy?.EnableMeleeAttackCheck(true);
    }

    public void FinishMeleeAttackCheck()
    {
        enemy?.EnableMeleeAttackCheck(false);

    }
}
