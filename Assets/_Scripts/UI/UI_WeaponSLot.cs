using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponSLot : MonoBehaviour
{
    public Image weaponIcon;
    public TextMeshProUGUI ammoText;


    void Awake()
    {
        weaponIcon = GetComponentInChildren<Image>();
        ammoText = GetComponentInChildren<TextMeshProUGUI>();
    }


    public void UpdateWeaponSlot(Weapon currentWeapon, bool activeWeapon)
    {
        if (weaponIcon == null || ammoText == null)
        {
            Debug.LogWarning("UI_WeaponSLot: weaponIcon or ammoText is not assigned!");
            return;
        }

        if (currentWeapon == null)
        {
            weaponIcon.color = Color.clear;
            ammoText.text = "";
            return;
        }

        if (currentWeapon.weaponData == null)
        {
            Debug.LogWarning("UI_WeaponSLot: currentWeapon.weaponData is null!");
            weaponIcon.color = Color.clear;
            ammoText.text = "";
            return;
        }

        Color newColor = activeWeapon ? Color.white : new Color(1f, 1f, 1f, .35f);

        weaponIcon.color = newColor;
        weaponIcon.sprite = currentWeapon.weaponData.weaponIcon;
        ammoText.text = currentWeapon.bulletsInMagazine + "/" + currentWeapon.totalReserveAmmo;
        ammoText.color = Color.white;
}

}
