using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class VirtualJoistick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    private Image bg;
    private Image joistickImg;
    private Vector2 inputVector;

    private void Awake()
    {
        bg = GetComponent<Image>();
        joistickImg = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bg.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bg.rectTransform.sizeDelta.y);

            inputVector = new Vector2(pos.x * 2, pos.y * 2);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            joistickImg.rectTransform.anchoredPosition = new Vector2(inputVector.x * (bg.rectTransform.sizeDelta.x/3), inputVector.y * (bg.rectTransform.sizeDelta.y / 3));
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joistickImg.rectTransform.anchoredPosition = inputVector;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
        {
            return inputVector.x;
        } else
        {
            return Input.GetAxisRaw("Horizontal");
        }
    }

    public float Vertical()
    {
        if (inputVector.y != 0)
        {
            return inputVector.y;
        }
        else
        {
            return Input.GetAxisRaw("Horizontal");
        }
    }
}
