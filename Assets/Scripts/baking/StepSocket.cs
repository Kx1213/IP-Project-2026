using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor))]
public class StepSocket : MonoBehaviour
{
    public RecipeManager manager;
    public int requiredStep;
    public string requiredTag;

    UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;

    private void Awake()
    {
        if (manager == null) manager = FindFirstObjectByType<RecipeManager>();
        socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
    }

    private void OnEnable() => socket.selectEntered.AddListener(OnEntered);
    private void OnDisable() => socket.selectEntered.RemoveListener(OnEntered);

    void OnEntered(SelectEnterEventArgs args)
    {
        if (!manager.IsStep(requiredStep)) return;

        var go = args.interactableObject.transform.gameObject;
        if (!go.CompareTag(requiredTag)) return;

        manager.CompleteStep(requiredStep);
    }
}
