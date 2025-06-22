using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_WeaponSelection : MonoBehaviour
{
    public UI_WeaponSelectionWindow[] selectionWeapon;
    [Header("Warning Info")]
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private float disaperSpeedWarningText = .25f;
    private float currentWarningAlpha;
    private float targetWarningAlpha;
    void Start()
    {
        selectionWeapon = GetComponentsInChildren<UI_WeaponSelectionWindow>();
    }


    void Update()
    {
        if (currentWarningAlpha > targetWarningAlpha)
        {
            currentWarningAlpha -= Time.deltaTime * disaperSpeedWarningText;
            warningText.color = new Color(1, 1, 1, currentWarningAlpha);
        }
    }

    public UI_WeaponSelectionWindow FindEmptySlot()
    {
        for (int i = 0; i < selectionWeapon.Length; i++)
        {
            if (selectionWeapon[i].IsEmpty()) // check if the slot is empty if yes return a slot
                return selectionWeapon[i];
        }

        return null;
    }


    public UI_WeaponSelectionWindow FindSloteWithWeaponOfType(Weapon_Data weaponData)
    {
        for (int i = 0; i < selectionWeapon.Length; i++)
        {
            if (selectionWeapon[i].weapon_Data == weaponData) // return 
                return selectionWeapon[i];
        }

        return null;
    }
    public List<Weapon_Data> SelectedWeaponData()
    {
        List<Weapon_Data> selectedData = new List<Weapon_Data>();
        foreach (UI_WeaponSelectionWindow weapon in selectionWeapon)
        {
            if (weapon.weapon_Data != null)
            {
                selectedData.Add(weapon.weapon_Data);
            }

        }

        return selectedData;
    }
    public void ShowWarningMessage(string message)
    {
        warningText.color = Color.red;
        warningText.text = message;


        currentWarningAlpha = warningText.color.a;
        targetWarningAlpha = 0;
    }
}
