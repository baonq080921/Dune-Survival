using System.Collections;
using UnityEngine;

public class Player_Health : HealthController
{

    private Player player;
    public bool isDead { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    public override void ReduceHealth(int damage)
    {
        base.ReduceHealth(damage);

        if (ShouldDie())
            Die();
        UI.instance.inGameUI.UpdateHealthUI(currentHealth, maxHealth);
    }

    protected override void Update()
    {
        base.Update();
        // if (isDead)
        // {
        //     StartCoroutine(BackOn());
        // }

    }


    // IEnumerator BackOn()
    // {
    //     yield return new WaitForSeconds(2f);
    //     Debug.Log("you are back on:");
    //     isDead = false;
    //     player.aim.enabled = true;
    //     player.ragdoll.RagdollActive(false);
    //     IncreaseHealth();

    // }


    private void Die()
    {
        isDead = true;
        player.aim.enabled = false;
        player.ragdoll.RagdollActive(true);
        GameManager.instance.GameOver();
    }
}
