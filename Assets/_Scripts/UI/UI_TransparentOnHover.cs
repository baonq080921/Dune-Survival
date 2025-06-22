using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.Collections;

public class UI_TransparentOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Dictionary<Image, Color> originalImageColor = new Dictionary<Image, Color>();
    private Dictionary<TextMeshProUGUI, Color> originalTextColor = new Dictionary<TextMeshProUGUI, Color>();

    private bool hasUIWeaponSlots;
    private PlayerWeaponController playerWeaponController;



    void Start()
    {
        hasUIWeaponSlots = GetComponentInChildren<UI_WeaponSLot>();
        if (hasUIWeaponSlots)
            playerWeaponController = FindObjectOfType<PlayerWeaponController>();
        foreach (var image in GetComponentsInChildren<Image>(true))
            {
                originalImageColor[image] = image.color;
            }

        foreach (var text in GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            originalTextColor[text] = text.color;
        }
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var image in originalImageColor.Keys)
        {
            var color = image.color;
            color.a = .15f;
            image.color = color;
        }

        foreach (var image in originalTextColor.Keys)
        {
            var color = image.color;
            color.a = .15f;
            image.color = color;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Restore the image color:
        foreach (var image in originalImageColor.Keys)
        {
            image.color = originalImageColor[image];

        }

        //Restore the text color:
        foreach (var image in originalTextColor.Keys)
        {
            image.color = originalTextColor[image];

        }
        playerWeaponController?.UpdateWeaponUI();
    }
}
