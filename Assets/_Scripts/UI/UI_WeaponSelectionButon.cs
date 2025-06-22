using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_WeaponSelectionButon : UI_Buttons
{

    private UI_WeaponSelection weaponSelectionUI;
    private UI_WeaponSelectionWindow emptySlot;
    [SerializeField] private Weapon_Data weaponData;
    [SerializeField] private Image weaponIcon;


    void OnValidate()
    {
        gameObject.name = "Button -Select Weapon: " + weaponData.weapongInfo;
    }
    public override void Start()
    {
        base.Start();
        weaponSelectionUI = GetComponentInParent<UI_WeaponSelection>();
        weaponIcon.sprite = weaponData.weaponIcon;
    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        weaponIcon.color = Color.yellow;
        emptySlot = weaponSelectionUI.FindEmptySlot();
        emptySlot?.UpdateSlotInfo(weaponData);

    }


    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        weaponIcon.color = Color.white;

        emptySlot?.UpdateSlotInfo(null);
        emptySlot = null;
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        weaponIcon.color = Color.white;


        //if there is any slots left:
        bool noMoreEmptySlots = weaponSelectionUI.FindEmptySlot() == null;
        bool noThisWeaponSlots = weaponSelectionUI.FindSloteWithWeaponOfType(weaponData) == null;

        if (noMoreEmptySlots && noThisWeaponSlots)
        {
            weaponSelectionUI.ShowWarningMessage("No Empty Slots!!!");
            return;
        }

        // reasgin the weapon slot window :
        UI_WeaponSelectionWindow slotsBusyWithThisWeapon = weaponSelectionUI.FindSloteWithWeaponOfType(weaponData);
        if (slotsBusyWithThisWeapon != null)
        {
            slotsBusyWithThisWeapon.SetWeaponSlot(null);
        }
        else
        {
            // basicly we need to assign the empty slot again and again
        emptySlot = weaponSelectionUI.FindEmptySlot();
        emptySlot?.SetWeaponSlot(weaponData);    
        }

        // assgin the weapon slot window:

        emptySlot = null;
    }
}
