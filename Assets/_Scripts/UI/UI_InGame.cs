using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_InGame : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Image healthBar;


    [Header("weapon")]
    [SerializeField] private UI_WeaponSLot[] weaponSLots_UI;

    [Header("Missions")]
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private TextMeshProUGUI missionDeatils;
    [SerializeField] private TextMeshProUGUI FinishLineText;

    [SerializeField] private GameObject missionToolTipParent;

    private bool toolTipActive = true;


    void Awake()
    {
        weaponSLots_UI = GetComponentsInChildren<UI_WeaponSLot>();

    }

    public void SwitchMissionToolTip()
    {
        toolTipActive = !toolTipActive;
        missionToolTipParent.SetActive(toolTipActive);


    }


    //Update the info on the mission we playing:
    public void UpdateUIMissionInfo(string missionText, string missionDeatils = "", string distanceToFinishLine = "")
    {
        this.missionText.text = missionText;
        this.missionDeatils.text = missionDeatils;
        this.FinishLineText.text = distanceToFinishLine;
    }
    // Update weapon UI:
    public void UpdateWeaponUI(List<Weapon> weaponslots, Weapon currentWeapon)
    {
        for (int i = 0; i < weaponSLots_UI.Length; i++)
        {
            if (i < weaponslots.Count)
            {
                //check if the weapon in the slot is equal to the currentweapon
                bool isActiveWeapon = weaponslots[i] == currentWeapon ? true : false;
                //Update slot
                weaponSLots_UI[i].UpdateWeaponSlot(weaponslots[i], isActiveWeapon);
            }
            else
            {
                weaponSLots_UI[i].UpdateWeaponSlot(null, false);
            }
        }
        
    }
    public void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
