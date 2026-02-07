using UnityEngine;

public class StepOilSalt : MonoBehaviour
{
    public RecipeManager manager;
    public float brushHoldTime = 1.0f;

    bool oiled, salted;
    float t;

    private void Awake()
    {
        if (manager == null) manager = FindFirstObjectByType<RecipeManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!manager.IsStep(4)) return;

        if (!oiled && other.CompareTag("Brush"))
        {
            t += Time.deltaTime;
            if (t >= brushHoldTime)
            {
                oiled = true;
                Debug.Log("✅ Oil applied");
            }
        }

        if (!salted && other.CompareTag("Salt"))
        {
            salted = true;
            Debug.Log("✅ Salt added");
        }

        if (oiled && salted)
        {
            manager.CompleteStep(4);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!oiled && other.CompareTag("Brush")) t = 0;
    }
}
