using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Buttons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler
{
    [Header("Mouse hover settings")]
    public float scaleRate = 1f;
    public float scaleAmount = 1.2f;
    private Vector3 defaultScale;
    private Vector3 targetScale;

    private Image btnImage;
    private TextMeshProUGUI tmpText;

    public virtual void Start()
    {

        defaultScale = transform.localScale;
        targetScale = defaultScale;
        btnImage = GetComponent<Image>();
        tmpText = GetComponentInChildren<TextMeshProUGUI>();

    }


    public virtual void Update()
    {

        if (Mathf.Abs(transform.localScale.x - targetScale.x) > .01f)
        {
            float scaleValue = Mathf.Lerp(transform.localScale.x, targetScale.x, Time.deltaTime * scaleRate);

            transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        }
        
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = defaultScale * scaleAmount;
        if(btnImage != null)
            btnImage.color = Color.yellow;
        if(tmpText != null)
            tmpText.color = Color.yellow;

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        ReturnDefaultColor();
    }

    private void ReturnDefaultColor()
    {
        targetScale = defaultScale;
        if(btnImage != null)
            btnImage.color = Color.white;
        if(tmpText != null)
            tmpText.color = Color.white;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        ReturnDefaultColor();
    }
}
