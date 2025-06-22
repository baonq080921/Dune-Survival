using UnityEngine;

public class Enemey_HitBox : HitBox
{
    private Enemy enemy;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
    }
    public override void TakeDamage(int damage)
    {
        int newDamage = Mathf.RoundToInt(damage * damageMultiplier);

        enemy.GetHit(newDamage);
    }
}
