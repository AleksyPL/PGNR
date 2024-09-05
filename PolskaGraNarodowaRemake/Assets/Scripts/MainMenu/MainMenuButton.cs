using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IMoveHandler, ISubmitHandler, IPointerMoveHandler, IDragHandler
{
    public UnityEvent UIFunction;
    public GameObject eventSystemGameObject;
    private EventSystem eventSystem;
    internal bool pointerOverTheButton;

    private void Start()
    {
        pointerOverTheButton = false;
        eventSystem = eventSystemGameObject.GetComponent<EventSystem>();
    }
    public void TriggerUIFunction()
    {
        UIFunction.Invoke();
        this.GetComponent<Animator>().SetBool("Selected", false);
        this.GetComponent<Animator>().SetBool("Highlighted", false);
        this.GetComponent<Animator>().SetBool("Normal", true);
    }
    public void SetThisButtonSelected()
    {
        eventSystem.SetSelectedGameObject(this.gameObject);
        this.GetComponent<Animator>().SetBool("Selected", true);
    }
    private void DeselectUIButton()
    {
        if(this.GetComponent<Button>().interactable && eventSystem.currentSelectedGameObject != this)
        {
            this.GetComponent<Animator>().SetBool("Selected", false);
            this.GetComponent<Animator>().SetBool("Highlighted", false);
            this.GetComponent<Animator>().SetBool("Normal", true);
        }
    }
    public void CheckIfPointerIsInside()
    {
        if (!pointerOverTheButton)
            DeselectUIButton();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerOverTheButton = true;
        SetThisButtonSelected();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerOverTheButton = false;
        DeselectUIButton();
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        if (pointerOverTheButton)
            SetThisButtonSelected();
    }
    public void OnMove(AxisEventData eventData)
    {
        if(eventSystem.currentSelectedGameObject != this)
            DeselectUIButton();
    }
    public void OnSubmit(BaseEventData eventData)
    {
        this.GetComponent<Animator>().SetBool("Selected", true);
    }
    public void OnDrag(PointerEventData eventData)
    {
        this.GetComponent<Animator>().SetBool("Selected", true);
    }
}
