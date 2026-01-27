using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class VRInputFieldActivator : MonoBehaviour, IPointerClickHandler
{
    private TMP_InputField inputField;

    void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inputField.Select();
        inputField.ActivateInputField();
    }
}
