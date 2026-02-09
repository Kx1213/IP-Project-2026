using UnityEngine;
using TMPro;

public class VRGazeTooltipRay : MonoBehaviour
{
    [Header("Ray Settings")]
    public float rayDistance = 10f;
    public LayerMask raycastLayers; // Include Interactable or Everything

    [Header("Dwell Settings")]
    public float dwellTime = 1.5f;

    [Header("UI")]
    public TMP_Text tooltipText;
    public GameObject tooltipPanel;

    private GazeTooltipTarget currentTarget;
    private float gazeTimer = 0f;
    private bool tooltipShown = false;

    void Start()
    {
        if (tooltipPanel)
            tooltipPanel.SetActive(false);
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Raycast using the assigned layers
        if (Physics.Raycast(ray, out hit, rayDistance, raycastLayers))
        {
            // Try to get the GazeTooltipTarget component
            GazeTooltipTarget target = hit.collider.GetComponent<GazeTooltipTarget>();

            if (target != null)
            {
                HandleGaze(target);
                return;
            }
        }

        // Nothing hit → reset gaze
        ResetGaze();
    }

    void HandleGaze(GazeTooltipTarget target)
    {
        if (target == currentTarget)
        {
            gazeTimer += Time.deltaTime;

            if (gazeTimer >= dwellTime && !tooltipShown)
            {
                ShowTooltip(target.displayName);
                tooltipShown = true;
            }
        }
        else
        {
            currentTarget = target;
            gazeTimer = 0f;
            tooltipShown = false;
            HideTooltip();
        }
    }

    void ShowTooltip(string text)
    {
        if (!tooltipPanel || !tooltipText) return;

        tooltipText.text = text;
        tooltipPanel.SetActive(true);
    }

    void HideTooltip()
    {
        if (tooltipPanel)
            tooltipPanel.SetActive(false);
    }

    void ResetGaze()
    {
        currentTarget = null;
        gazeTimer = 0f;
        tooltipShown = false;
        HideTooltip();
    }
}
