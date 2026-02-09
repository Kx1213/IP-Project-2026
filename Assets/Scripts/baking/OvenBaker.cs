using System.Collections;
using UnityEngine;

public class OvenBaker : MonoBehaviour
{
    public CookingStepManager manager;

    public float bakeSeconds = 10f; // demo time
    public GameObject rawVisual;
    public GameObject bakedVisual;

    private bool baking;

    public void PressBake()
    {
        if (baking) return;

        if (!manager.IsStep(CookingStepManager.Step.Bake40Min))
            return;

        baking = true;
        StartCoroutine(BakeRoutine());
    }

    private IEnumerator BakeRoutine()
    {
        Debug.Log("Baking...");
        yield return new WaitForSeconds(bakeSeconds);

        if (rawVisual) rawVisual.SetActive(false);
        if (bakedVisual) bakedVisual.SetActive(true);

        manager.AdvanceStep(CookingStepManager.Step.Bake40Min);
        baking = false;
    }
}
