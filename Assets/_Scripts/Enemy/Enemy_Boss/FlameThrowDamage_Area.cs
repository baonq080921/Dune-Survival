using UnityEngine;

public class FlameThrowDamage_Area : MonoBehaviour
{
    private Enemy_Boss enemy;
    private float damageCoolDown;
    private int FlameThrowDamage;
    private float lastTimeDamaged;

    void Awake()
    {
        enemy = GetComponentInParent<Enemy_Boss>();
        FlameThrowDamage = enemy.flameDamage;
        damageCoolDown = enemy.flameDamageCoolDown;
    }


    void OnTriggerStay(Collider other)
    {
        if (enemy.flameThrowActive == false)
            return;
        if (Time.time - lastTimeDamaged < damageCoolDown)
            return;


        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(FlameThrowDamage);
            lastTimeDamaged = Time.time;
            damageCoolDown = enemy.flameDamageCoolDown;

            //Slow down the player if hit
            Player_HitBox playerHitBox = other.GetComponent<Player_HitBox>();
            if (playerHitBox != null)
            {
                playerHitBox.SetSlow(true, 0.5f);
            }

        }

    }

    void OnTriggerExit(Collider other)
    {
        Player_HitBox playerHitBox = other.GetComponent<Player_HitBox>();
        if (playerHitBox != null)
        {
            playerHitBox.SetSlow(false); // Restore speed when leaving area
        }
    }
}
