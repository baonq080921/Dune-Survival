using UnityEngine;

public class HealthController : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;

    private bool isDead;
    protected virtual void Awake()
    {

        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        
    }


    public virtual void ReduceHealth(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            currentHealth = 0;
    }

    public virtual void IncreaseHealth()
    {
        currentHealth++;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
  
    }


    public bool ShouldDie() => currentHealth <= 0;
    



}
