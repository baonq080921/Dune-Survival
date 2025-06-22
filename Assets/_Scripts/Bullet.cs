using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float impactForce;
    public int bulletdamge;

    private BoxCollider cd;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;
    private LayerMask allyLayerMask;

    [SerializeField] private GameObject bulletImpactFX;


    private Vector3 startPosition;
    private float flyDistance;
    private bool bulletDisabled;

    protected virtual void Awake()
    {
        cd = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void BulletSetup(LayerMask allyLayerMask,int bulletdamge ,float flyDistance = 100f, float impactForce = 100f)
    {
        this.impactForce = impactForce;
        this.allyLayerMask = allyLayerMask;
        this.bulletdamge = bulletdamge;

        bulletDisabled = false;
        cd.enabled = true;
        meshRenderer.enabled = true;

        trailRenderer.time = .25f;
        startPosition = transform.position;
        this.flyDistance = flyDistance + .5f; // magic number .5f is a length of tip of the laser ( Check method UpdateAimVisuals() on PlayerAim script) ;
    }

    protected virtual void Update()
    {
        FadeTrailIfNeeded();
        DisableBulletIfNeeded();
        ReturnToPoolIfNeeded();
    }

    protected void ReturnToPoolIfNeeded()
    {
        if (trailRenderer.time < 0)
            ReturnBulletToPool();
    }
    protected void DisableBulletIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisabled)
        {
            cd.enabled = false;
            meshRenderer.enabled = false;
            bulletDisabled = true;
        }
    }
    protected void FadeTrailIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5f)
            trailRenderer.time -= 2 * Time.deltaTime; // magic number 2 is choosen trhou testing
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {

        if (FriendlyFireEnable() == false)
        {
            // if bullet hits ally layer
            //then return;
            //Use a bitwise and to check if the collison the layer is in the allyLayerMask
            if ((allyLayerMask.value & (1 << collision.gameObject.layer)) > 0)
            {
                return;
            }

        }

        CreateImpactFx(collision);
        ReturnBulletToPool();

        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
        damagable?.TakeDamage(bulletdamge);


        ApplyBulletImapctToEnemy(collision);
    }

    private void ApplyBulletImapctToEnemy(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            Vector3 force = rb.linearVelocity.normalized * impactForce;
            Rigidbody hitRigidbody = collision.collider.attachedRigidbody;
            enemy.BulletImpact(force, collision.contacts[0].point, hitRigidbody);
        }
    }

    protected void ReturnBulletToPool() => ObjectPool.instance.ReturnObject(gameObject);


    protected void CreateImpactFx(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];

            GameObject newImpactFx = ObjectPool.instance.GetObject(bulletImpactFX, transform);
            newImpactFx.transform.position = contact.point;

            ObjectPool.instance.ReturnObject(newImpactFx, 1);
        }
    }

    private bool FriendlyFireEnable() => GameManager.instance.friendlyFire;
}
