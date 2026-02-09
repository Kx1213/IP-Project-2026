using UnityEngine;

public class SaltAdder : MonoBehaviour
{
    public CookingStepManager manager;

    public float saltNeeded = 2f;
    private float saltAdded;

    public float addPerSecond = 0.5f;
    public float minShakeSpeed = 0.8f;

    private void OnTriggerStay(Collider other)
    {
        if (!manager.IsStep(CookingStepManager.Step.SprinkleSalt))
            return;

        if (!other.CompareTag("SaltShaker"))
            return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        float speed = rb.linearVelocity.magnitude;
        if (speed < minShakeSpeed) return;

        saltAdded += addPerSecond * Time.deltaTime;

        if (saltAdded >= saltNeeded)
            manager.AdvanceStep(CookingStepManager.Step.SprinkleSalt);
    }
}
