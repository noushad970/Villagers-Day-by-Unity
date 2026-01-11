using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHoldDebug : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler
{
    private bool isPressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        CharacterMovement.isRunPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CharacterMovement.isRunPressed = false;
    }

}
