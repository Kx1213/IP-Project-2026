using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor))]
public class PlateFinishGate : MonoBehaviour
{
    public CookingStepManager manager;
    public string requiredTag = "Potato";

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
        if (!manager) return;

        // 必须是在 ServeOnPlate 这一步
        if (!manager.IsStep(CookingStepManager.Step.ServeOnPlate))
            return;

        Transform obj = args.interactableObject.transform;

        if (!string.IsNullOrEmpty(requiredTag) && !obj.CompareTag(requiredTag))
            return;

        manager.FinishGame(CookingStepManager.Step.ServeOnPlate);
    }
}
