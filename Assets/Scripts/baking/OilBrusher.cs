using UnityEngine;

public class OilBrusher : MonoBehaviour
{
    public CookingStepManager manager;

    [Header("Brushing")]
    public float brushNeeded = 1.5f;     
    public float minBrushSpeed = 0.15f; 
    public float addPerSecond = 1f;

    private float brushed;
    private Vector3 lastBrushPos;
    private bool hasLast;

    private void OnTriggerStay(Collider other)
    {
        if (!manager || !manager.IsStep(CookingStepManager.Step.BrushWithOilOrButter))
            return;


        if (!other.CompareTag("Brush"))
            return;


        Vector3 pos = other.transform.position;

        if (!hasLast)
        {
            lastBrushPos = pos;
            hasLast = true;
            return;
        }

        float speed = (pos - lastBrushPos).magnitude / Time.deltaTime;
        lastBrushPos = pos;

        // Must be moving to count
        if (speed < minBrushSpeed) return;

        brushed += addPerSecond * Time.deltaTime;

        if (brushed >= brushNeeded)
        {
            manager.AdvanceStep(CookingStepManager.Step.BrushWithOilOrButter);
            enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Brush"))
            hasLast = false;
    }
}
