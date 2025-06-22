using UnityEngine;

public class HitBox : MonoBehaviour,IDamagable
{
    [SerializeField] protected float damageMultiplier = 1f;
    protected virtual void Awake()
    {



    }


    public virtual void TakeDamage(int damage)
    {
    }
}
