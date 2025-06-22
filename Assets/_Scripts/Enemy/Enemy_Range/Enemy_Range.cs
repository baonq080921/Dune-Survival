using UnityEngine;

public class Enemy_Range : Enemy
{
    public Transform weaponHolder;
    public Transform gunpoint;

    public float fireRate = 1f;  // BUllets fire per second

    public GameObject bulletPrefab;
    public int bulletDamageEnemy;
    public float bulletSpeed = 10f;

    public int bulletToShoot = 5; // Bullets to shoot before weapon goes on cooldown
    public float weaponCooldown = 1.5f; // Weapon cooldown After all the bullets are shot



    public IdleState_Range idleState { get; private set; }
    public MoveState_Range moveState { get; private set; }
    public BattleState_Range battleState { get; private set; }

    public DeadState_Range deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        // Initialize the states
        idleState = new IdleState_Range(this, stateMachine, "Idle");
        moveState = new MoveState_Range(this, stateMachine, "Move");
        battleState = new BattleState_Range(this, stateMachine, "Battle");
        deadState = new DeadState_Range(this, stateMachine, "Idle");

    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
        visuals.SetupLook();
    }


    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public override void Die()
    {
        base.Die();
        if (stateMachine.currentState != deadState)
            stateMachine.ChangeState(deadState);
    }


    public bool IsPlayerOutOfRange()
    {
        if (Vector3.Distance(player.position, transform.position) > aggresionRange)
        {
            return true;
        }
        return false;
    }



    public override void EnterBattleMode()
    {
        if (inBattleMode) return;
        base.EnterBattleMode();

        stateMachine.ChangeState(battleState);

    }


    public void FireSingleBullet()
    {

        if (isDead) return;

        if (stateMachine.currentState == deadState) return;
        anim.SetTrigger("Shoot");

        Vector3 bulletDirection = ((player.position + Vector3.up) - gunpoint.position).normalized;

        GameObject newBullet = ObjectPool.instance.GetObject(bulletPrefab,transform);
        newBullet.transform.position = gunpoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunpoint.forward);



        newBullet.GetComponent<Bullet>().BulletSetup(whatisAlly,bulletDamageEnemy,flyDistance: 0.1f);
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
        rbNewBullet.mass = 20 / bulletSpeed;
        rbNewBullet.linearVelocity = bulletDirection * bulletSpeed;

    }
}
