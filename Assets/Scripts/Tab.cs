using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Coffee.UIExtensions;

//used this instead of the base class Button to have more controll over the pressed state and have a cleaner inspector.
public class Tab : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region Inspector Variables
    [SerializeField] private Image background;
    [SerializeField] private Color backgroundDefaultColor = Color.white;
    [SerializeField] private Color backgroundHoverColor = Color.gray;
    [SerializeField] private Text text;
    [SerializeField] private Color textDefaultColor = Color.black;
    [SerializeField] private Color textSelectedColor = Color.white;
    [SerializeField] private UIGradient gradient;
    #endregion

    #region Unity Events
    [SerializeField] private UnityEvent onClick;
    [SerializeField] private UnityEvent onSelected;
    [SerializeField] private UnityEvent onLoseFocus;
    #endregion

    public Action<Tab> onClickEvent;

    public void Select()
    {
        gradient.enabled = false;
        text.color = textSelectedColor;
        onSelected.Invoke();
    }

    public void Leave()
    {
        gradient.enabled = true;
        text.color = textDefaultColor;
        onLoseFocus.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(onClickEvent != null)
            onClickEvent.Invoke(this); //event to be used by code - unity events are quite heavy and slow...

        onClick.Invoke(); //unity event to be used with inspector, that's why they're nice!
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.color = backgroundHoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        background.color = backgroundDefaultColor;
    }
}