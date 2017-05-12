using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private InputField _inputField;

	/*
     * Method:
     *   Start
     * 
     * Description:
     *   Runs when the object is created, after all Start methods
     *   are run. We want to initialize the size of the panel, so
     *   that it takes up a nice portion of the screen. We also need
     *   to get references to the objects used by the rest of the
     *   class's methods.
     */
	private void Start ()
    {

    }

    /*
     * Method:
     *   OnPointerEnter
     * 
     * Description:
     *   Callback for a pointer entering the space. In our case,
     *   we want to increase the alpha when a pointer enters, so
     *   the chat panel can be easily seen.
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
    }

    /*
     * Method:
     *   OnPointerExit
     * 
     * Description:
     *   Callback for a pointer exiting the space. In our case,
     *   we want to decrease the alpha when a pointer leaves, so
     *   the chat panel is not in the way of seeing the game.
     */
    public void OnPointerExit(PointerEventData eventData)
    {
        // Check to see if the message input field is focused.
        // If it is, then we don't want to decrease the alpha
        // or make the panel not interactable.
        if (_inputField.isFocused)
        {
            return;
        }
        
        // The input field is not focused, so it's safe to dim
        // the chat panel.
        _canvasGroup.alpha = 0.15f;
        _canvasGroup.interactable = false;
    }
}
