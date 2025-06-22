using UnityEngine;
using System.Collections;

public class Player_HitBox : HitBox
{
    private Player player;
    private float? originalWalkSpeed = null;
    private float? originalRunSpeed = null;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponentInParent<Player>();
    }


    public override void TakeDamage(int damage)
    {
        Debug.Log("player health --.Damage take in" + damage);
        player.health.ReduceHealth(damage);
    }


public void SetSlow(bool slow, float slowAmount = 1f)
{
    var movement = player.GetComponent<PlayerMovement>();
    if (movement != null)
    {
        if (slow)
        {
            if (originalWalkSpeed == null) originalWalkSpeed = movement.walkSpeed;
            if (originalRunSpeed == null) originalRunSpeed = movement.runSpeed;
            movement.walkSpeed = originalWalkSpeed.Value * slowAmount;
            movement.runSpeed = originalRunSpeed.Value * slowAmount;
        }
        else
        {
            if (originalWalkSpeed != null) movement.walkSpeed = originalWalkSpeed.Value;
            if (originalRunSpeed != null) movement.runSpeed = originalRunSpeed.Value;
            originalWalkSpeed = null;
            originalRunSpeed = null;
        }
    }
}

}
