using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor))]
public class SocketStepGate : MonoBehaviour
{
    public CookingStepManager manager;

    public CookingStepManager.Step requiredStep;
    public CookingStepManager.Step stepToAdvanceFrom;

    public string requiredTag;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;

    private void Awake()
    {
        socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
    }

    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!manager.IsStep(requiredStep))
            return;

        Transform obj = args.interactableObject.transform;

        if (!string.IsNullOrEmpty(requiredTag) && !obj.CompareTag(requiredTag))
            return;

        manager.AdvanceStep(stepToAdvanceFrom);
    }
}
