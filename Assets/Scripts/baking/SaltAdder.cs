using UnityEngine;

public class SaltAdder : MonoBehaviour
{
    public CookingStepManager manager;

    public float saltNeeded = 2f;
    public float addPerSecond = 0.5f;

    private float saltAdded;

    private void OnTriggerStay(Collider other)
    {
        if (!manager || !manager.IsStep(CookingStepManager.Step.SprinkleSalt))
            return;

        if (!other.CompareTag("SaltShaker"))
            return;

        saltAdded += addPerSecond * Time.deltaTime;

        if (saltAdded >= saltNeeded)
            manager.AdvanceStep(CookingStepManager.Step.SprinkleSalt);
    }
}
