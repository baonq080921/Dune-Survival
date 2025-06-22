using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy_WeaponModel : MonoBehaviour
{

    public Enemy_MeleeWeaponType weaponType;
    public AnimatorOverrideController overrideController;
    public Enemy_MeleeWeaponData weaponData;

    [SerializeField] private GameObject[] trailEffects;

    [Header("Damage atributes")]
    public Transform[] damagePoint;
    public float attackRaidus;

    public int attackDamage;


    [ContextMenu("Assign damage point transform")]
    private void GetDamgePoints()
    {
        damagePoint = new Transform[trailEffects.Length];

        for (int i = 0; i < trailEffects.Length; i++)
        {
            damagePoint[i] = trailEffects[i].transform;
        }
    }

    public void EnableTrailEffect(bool enable)
    {
        foreach (var effect in trailEffects)
        {
            effect.SetActive(enable);
        }
    }

    void OnDrawGizmos()
    {
        if (damagePoint.Length > 0)
        {
            foreach (Transform point in damagePoint)
            {
                Gizmos.DrawWireSphere(point.position, attackRaidus);
            }
        }
    }
}
