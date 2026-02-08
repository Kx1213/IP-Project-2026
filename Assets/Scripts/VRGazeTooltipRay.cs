using UnityEngine;
using TMPro;

public class VRGazeTooltipRay : MonoBehaviour
{
    [Header("Ray Settings")]
    public float rayDistance = 10f;
    public LayerMask interactableLayer;

    [Header("Dwell Settings")]
    public float dwellTime = 1.5f;

    [Header("UI")]
    public TMP_Text tooltipText;
    public GameObject tooltipPanel;

    GazeTooltipTarget currentTarget;
    float gazeTimer = 0f;

    void Start()
    {
        tooltipPanel.SetActive(false);
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            GazeTooltipTarget target = hit.collider.GetComponent<GazeTooltipTarget>();

            if (target != null)
            {
                if (target == currentTarget)
                {
                    gazeTimer += Time.deltaTime;

                    if (gazeTimer >= dwellTime)
                    {
                        ShowTooltip(target.displayName);
                    }
                }
                else
                {
                    currentTarget = target;
                    gazeTimer = 0f;
                    HideTooltip();
                }

                return;
            }
        }

        // If ray misses or hits non-target
        ResetGaze();
    }

    void ShowTooltip(string text)
    {
        tooltipText.text = text;
        tooltipPanel.SetActive(true);
    }

    void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }

    void ResetGaze()
    {
        currentTarget = null;
        gazeTimer = 0f;
        HideTooltip();
    }
}
