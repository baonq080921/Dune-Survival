using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_WeaponSelectionWindow : MonoBehaviour
{
    public Weapon_Data weapon_Data;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponInfo;



    void Start()
    {
        UpdateSlotInfo(null);
    }

    public void SetWeaponSlot(Weapon_Data newWeaponData)
    {
        weapon_Data = newWeaponData;
        UpdateSlotInfo(weapon_Data);
    }
    public void UpdateSlotInfo(Weapon_Data weaponData)
    {
        if (weaponData == null)
        {
            weaponIcon.color = Color.clear;
            weaponInfo.text = "Select a weapon";
            return;
        }

        weaponIcon.color = Color.white;
        weaponIcon.sprite = weaponData.weaponIcon;
        weaponInfo.text = weaponData.weapongInfo;
    }


    public bool IsEmpty() => weapon_Data == null;

}
