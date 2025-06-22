using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyBoss_Visual : MonoBehaviour
{
    private Enemy_Boss enemy;
    [SerializeField] private float landingOffSet = 1f;
    [SerializeField] private GameObject[] batteries;
    [SerializeField] private GameObject[] weaponTrails;
    [SerializeField] private float initialBatteryScaleY = .2f ;
    [SerializeField] private ParticleSystem landingZoneFx;

    private float discharggSpeed;
    private float rechargeSpeed;

    private bool isRecharging;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        enemy = GetComponent<Enemy_Boss>();
        landingZoneFx.transform.parent = null;
        landingZoneFx.Stop();
        ResetBatteries();

    }

    // Update is called once per frame
    void Update()
    {

        UpdateBatteriesScale();

    }


    public void EnableWeaponTrail(bool active)
    {

        if (weaponTrails.Length <= 0)
        {
            return;
        }
        foreach (GameObject trail in weaponTrails)
        {
            trail.gameObject.SetActive(active);
        }
    }

    private void UpdateBatteriesScale()
    {
        if (batteries.Length < 0)
        {
            return;
        }

        foreach (GameObject battery in batteries)
        {
            if (battery.activeSelf)
            {
                float scaleChange = (isRecharging ? rechargeSpeed : -discharggSpeed) * Time.deltaTime;


                float newScaleY = Mathf.Clamp(battery.transform.localScale.y + scaleChange, 0, initialBatteryScaleY);

                battery.transform.localScale = new Vector3(0.15f, newScaleY, 0.15f);


                if (battery.transform.localScale.y <= 0)
                    battery.SetActive(false);
            }
        }
    }
    public void ResetBatteries()
    {
        isRecharging = true;

        rechargeSpeed = initialBatteryScaleY / enemy.abilityCoolDown;
        discharggSpeed = initialBatteryScaleY / (enemy.flameThrowDuration * .75f);

        foreach (GameObject battery in batteries)
        {
            battery.SetActive(true);
        }
    }


    public void DischargeBatteries() => isRecharging = false;


    public void PlaceLandingZone(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        Vector3 offSet = dir * landingOffSet;
        landingZoneFx.transform.position = target + offSet;

        landingZoneFx.Clear();
        var mainModule = landingZoneFx.main;
        // Set the duration as needed, for example:
        mainModule.duration = enemy.travelTimeToTarget;
        Debug.Log("time travel to target" + enemy.travelTimeToTarget);
        landingZoneFx.Play();
    }
}
